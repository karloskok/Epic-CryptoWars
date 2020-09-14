using UnityEngine;

public class HeroBombManager : HeroManager
{
    [SerializeField] private BombManager bombPrefab;

    public override bool CanBeAsTarget(HeroManager target)
    {
        return true;
    }

    public override void OnFight()
    {
        BombManager bomb = Instantiate(bombPrefab);
        bomb.Shot(transform.position, TargetObject);
    }
}