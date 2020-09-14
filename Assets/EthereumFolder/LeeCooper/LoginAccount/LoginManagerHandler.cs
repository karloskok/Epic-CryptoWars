using Game.UIManagerScripts.Login;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManagerHandler : MonoBehaviour
{

    public InputField key;
    public InputField key2;

    public InputField accountName;


    public LoginPanelManager loginPanelManager;

    public Animator LoginPanel;
    public TimedAction blur;

    public PlayerKeysHolder playerKeysHolder;

    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);

        playerKeysHolder.Name = "guest";
        playerKeysHolder.playerEthereumAccount = "";
        playerKeysHolder.playerEthereumPrivateKey = "";
        playerKeysHolder.Level = -1;
        playerKeysHolder.Wins= -1;
        playerKeysHolder.Defeats= -1;
        playerKeysHolder.Score = -1;
        playerKeysHolder.Coins = -1;

    }


    public void Login()
    {
        Account account = GameObject.FindObjectOfType<Account>();
        string pKey = key.text;

        account.importAccountFromPrivateKey(pKey);
        //TODO: check login - register
    }

    public void LoginComplete()
    {
        LoginPanel.Play("Login to Loading");
        blur.StartIEnumerator();
        StartCoroutine(LoadMainMenu(3f));
    }

    public IEnumerator LoadMainMenu(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OpenNewAccountPanel()
    {
        loginPanelManager.PanelAnim(1);
    }

    public void CreateNewAccount()
    {
        Account account = GameObject.FindObjectOfType<Account>();
        string pKey = key2.text;

        account.importAccountFromPrivateKeyRegister(pKey);
        string name = accountName.text;

        if (string.IsNullOrEmpty(name))
        {
            UnityEngine.Debug.Log("Name is empty");
        }
        else
        {
            account.CreatePlayer(name);
        }

    }
}
