using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameObjectManager : MonoBehaviour
{
    public SimpleVector2 Position { get; protected set; }
    public int Lives { get; private set; }
    public int Force { get; private set; }
    public GameObjectManager TargetObject { get; protected set; }
    public bool IsBomb { get; protected set; }
    [SerializeField] protected int startLives = 5;
    [SerializeField] protected int startForce = 1;
    [SerializeField] private Transform livesProgress;
    [SerializeField] private int minTargetDistance = 1;
    [SerializeField] private bool isBomb = false;
    private int minTargetDistancePow;

    public bool IsAttacking = false;

    public virtual void Init(int groupIndex, SimpleVector2 initPosition, GameObjectManager initTargetObject, Material groupMaterial)
    {
        Lives = startLives;
        Force = startForce;
        Position = initPosition;
        TargetObject = initTargetObject;
        transform.position = new Vector3(initPosition.x, 0.0f, initPosition.z);
        if (livesProgress)
        {
            livesProgress.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial = groupMaterial;
            //init color of units
        }
        minTargetDistancePow = minTargetDistance * minTargetDistance;
        IsBomb = isBomb;
    }
    public void SetTargetObject(GameObjectManager targetObject)
    {
        TargetObject = targetObject;
    }
    public virtual bool Damage(int force)
    {
        if(startLives == 0)
        {
            return false;
        }
        Lives -= force;
        if (Lives > 0)
        {
            livesProgress.localScale = new Vector3((float)Lives / (float)startLives, 1.0f, 1.0f);
        }
        else
        {
            livesProgress.localScale = new Vector3(0.0f, 1.0f, 1.0f);
        }
        return Lives <= 0;
    }
    public virtual void Remove()
    {
        if(gameObject.GetComponent<Animator>())
            gameObject.GetComponent<Animator>().SetTrigger("Die");
        Destroy(gameObject, 2f);
    }

    public void RemoveTargetObject()
    {
        if (TargetObject)
        {
            TargetObject.Remove();
            TargetObject = null;
        }
    }

    public bool CanDamageTarget()
    {
        if(TargetObject && GetType() == typeof(HeroBombManager))
        Debug.Log((Position - TargetObject.Position).SqrMagnitude +  "  " + minTargetDistancePow);
        return TargetObject && (Position - TargetObject.Position).SqrMagnitude < minTargetDistancePow + 1;
    }
    public abstract void OnFight();
    public abstract bool CanBeAsTarget(HeroManager target);
    public abstract void RemoveFromGroup(Group group);
}
