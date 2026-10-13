using UnityEngine;

public sealed class AttackAnimationEvents : MonoBehaviour
{
    private MeleeAttack _meleeAttack;
    private MagicAttack _magicAttack;

    private void Awake()
    {
        _meleeAttack = GetComponentInParent<MeleeAttack>();
        _magicAttack = GetComponentInParent<MagicAttack>();
    }

    public void MeleeHit()
    {
        _meleeAttack?.ResolveHit();
    }

    public void MagicRelease()
    {
        _magicAttack?.ReleaseProjectile();
    }
}