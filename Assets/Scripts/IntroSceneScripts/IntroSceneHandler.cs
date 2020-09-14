using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartLoginScene(3f));
    }

    IEnumerator StartLoginScene(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene("LoginScene");

    }
}
