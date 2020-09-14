using UnityEngine;
using System.Collections;

public class LoaderInGameManager : MonoBehaviour
{
    public GameObject canvas;
    // Game Instance Singleton


    private void Awake()
    {
        //canvas = transform.GetChild(0).gameObject;
    }

    public void OpenLoading()
    {
        canvas.SetActive(true);
    }
    public void CloseLoading()
    {
        canvas.SetActive(false);
    }
}