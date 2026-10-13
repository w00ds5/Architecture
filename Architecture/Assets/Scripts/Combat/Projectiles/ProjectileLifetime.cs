using UnityEngine;

public class ProjectileLifetime : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float lifetime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}