using UnityEngine;

public class SimpleHeroManager : HeroManager
{
    [SerializeField] private Animation fightAnimation;
    [SerializeField] private Animator anim;

    public override bool CanBeAsTarget(HeroManager target)
    {
        return true;
    }

    public override void OnFight()
    {
        //IsAttacking = true;
        if (anim)
        {
            anim.SetTrigger("Fight");
        }
        else
        {
            //TargetObject;
            fightAnimation.Play();
        }

    }
}