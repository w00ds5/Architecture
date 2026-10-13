using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ProjectileImpact : MonoBehaviour
{
    private SphereCollider _collider;
    private DamageInfo _damage;
    private LayerMask _targetLayers;

    private bool _initialized;
    private bool _hasHit;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    public void Initialize(
        DamageInfo damage,
        LayerMask targetLayers,
        Transform owner)
    {
        _damage = damage;
        _targetLayers = targetLayers;

        Collider[] ownerColliders =
            owner.GetComponentsInChildren<Collider>();

        foreach (Collider ownerCollider in ownerColliders)
            Physics.IgnoreCollision(_collider, ownerCollider);

        _initialized = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_initialized || _hasHit)
            return;

        _hasHit = true;

        int hitLayer = collision.collider.gameObject.layer;

        if ((_targetLayers.value & (1 << hitLayer)) != 0)
        {
            IDamageable target =
                collision.collider.GetComponentInParent<IDamageable>();

            if (target != null)
                target.TakeDamage(_damage);
        }

        Destroy(gameObject);
    }
}