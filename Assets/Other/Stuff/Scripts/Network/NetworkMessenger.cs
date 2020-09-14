using UnityEngine;
using System;


public class NetworkMessenger : MonoBehaviour
{
    public Action OnStartGame;
    public Action<float> OnPing;
    public Action<int, long, SimpleVector2> OnAddOtherHeroToWaitingList;

    [NetworkRPC]
    public void AddOtherHeroToWaitingList(int index, long addTime, int x, int z)
    {
        OnAddOtherHeroToWaitingList?.Invoke(index, addTime, new SimpleVector2(x, z));
    }
    [NetworkRPC]
    public void StartGame()
    {
        OnStartGame?.Invoke();
    }
    [NetworkRPC]
    public void Ping(float gameTime)
    {
        OnPing?.Invoke(gameTime);
    }

    public void CleraActions()
    {
        OnStartGame = null;
        OnAddOtherHeroToWaitingList = null;
    }
}

