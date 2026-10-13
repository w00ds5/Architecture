using System;
using UnityEngine;
using UnityEngine.AI;

public sealed class NavMeshPathfinder : MonoBehaviour
{
    [SerializeField, Min(0.05f)]
    private float repathInterval = 0.2f;

    [SerializeField, Min(0.01f)]
    private float cornerReachDistance = 0.15f;

    [SerializeField, Min(0.1f)]
    private float sampleDistance = 1.5f;

    private NavMeshPath _path;
    private NavMeshPath _probePath;
    private Vector3[] _corners = Array.Empty<Vector3>();
    private int _cornerIndex;
    private float _nextRepathTime;

    private void Awake()
    {
        _path = new NavMeshPath();
        _probePath = new NavMeshPath();
    }

    private void OnDisable()
    {
        ClearPath();
    }

    public Vector3 GetDirection(Vector3 destination)
    {
        if (!isActiveAndEnabled)
            return Vector3.zero;

        if (Time.time >= _nextRepathTime)
            RebuildPath(destination);

        while (_cornerIndex < _corners.Length)
        {
            Vector3 direction =
                _corners[_cornerIndex] - transform.position;

            direction.y = 0f;

            if (direction.sqrMagnitude
                > cornerReachDistance * cornerReachDistance)
            {
                return direction.normalized;
            }

            _cornerIndex++;
        }

        return Vector3.zero;
    }

    public void ClearPath()
    {
        _corners = Array.Empty<Vector3>();
        _cornerIndex = 0;
        _nextRepathTime = 0f;
    }

    private void RebuildPath(Vector3 destination)
    {
        _nextRepathTime = Time.time + repathInterval;
        _corners = Array.Empty<Vector3>();
        _cornerIndex = 0;

        if (!NavMesh.SamplePosition(
                transform.position,
                out NavMeshHit start,
                sampleDistance,
                NavMesh.AllAreas)
            || !NavMesh.SamplePosition(
                destination,
                out NavMeshHit end,
                sampleDistance,
                NavMesh.AllAreas))
        {
            return;
        }

        if (!NavMesh.CalculatePath(
                start.position,
                end.position,
                NavMesh.AllAreas,
                _path)
            || _path.status == NavMeshPathStatus.PathInvalid)
        {
            return;
        }

        _corners = _path.corners;

        // Первая точка — начало маршрута.
        _cornerIndex = _corners.Length > 1 ? 1 : 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        for (int i = 1; i < _corners.Length; i++)
        {
            Gizmos.DrawLine(
                _corners[i - 1] + Vector3.up * 0.1f,
                _corners[i] + Vector3.up * 0.1f);
        }
    }

    public bool TryFindReachablePoint(
        Vector3 candidate,
        out Vector3 point)
    {
        point = Vector3.zero;

        if (!isActiveAndEnabled
            || !NavMesh.SamplePosition(
                transform.position,
                out NavMeshHit start,
                sampleDistance,
                NavMesh.AllAreas)
            || !NavMesh.SamplePosition(
                candidate,
                out NavMeshHit end,
                sampleDistance,
                NavMesh.AllAreas))
        {
            return false;
        }

        if (!NavMesh.CalculatePath(
                start.position,
                end.position,
                NavMesh.AllAreas,
                _probePath)
            || _probePath.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }

        point = end.position;
        return true;
    }
}