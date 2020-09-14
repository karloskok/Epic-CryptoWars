using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTowerManager : TowerManager
{
    [SerializeField] private float shotVelocity = 5.0f;
    [SerializeField] private BulletManager bulletPrefab;

    public override void OnFight()
    {
        BulletManager bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.Shot(shotVelocity, TargetObject);
        //TargetObject;
    }
    public override bool CanBeAsTarget(HeroManager target)
    {
        return true;
    }
}
