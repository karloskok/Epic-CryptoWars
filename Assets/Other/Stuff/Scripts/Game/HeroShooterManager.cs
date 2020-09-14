using UnityEngine;

public class HeroShooterManager : HeroManager
{
    [SerializeField] private float shotVelocity = 5.0f;
    [SerializeField] private BulletManager bulletPrefab;
    [SerializeField] private Transform shotPivot;

    [SerializeField] private Animator anim;

    public override bool CanBeAsTarget(HeroManager target)
    {
        return true;
    }

    public override void OnFight()
    {
        BulletManager bullet = Instantiate(bulletPrefab, shotPivot.position, shotPivot.rotation);
        bullet.Shot(shotVelocity, TargetObject);

        if (anim)
        {
            anim.SetTrigger("Fight");
        }
    }
}