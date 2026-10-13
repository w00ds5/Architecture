using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
public sealed class EnemyDespawnOnDeath : MonoBehaviour
{
    private CharacterHealth _health;

    private void Awake()
    {
        _health = GetComponent<CharacterHealth>();
    }

    private void OnEnable()
    {
        _health.Died += Despawn;
    }

    private void OnDisable()
    {
        _health.Died -= Despawn;
    }

    private void Despawn()
    {
        Destroy(gameObject, 4f);
    }
}