using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InitUIOnGame : MonoBehaviourPunCallbacks
{

    public Text[] localPlayerName;
    public Text[] enemyPlayerName;

	public GameObject startUI;
	// Start is called before the first frame update


	IEnumerator ShowStartUI()
	{
		for (int i = 0; i < localPlayerName.Length; i++)
		{
			if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[i].UserId)
			{
				localPlayerName[0].text = PhotonNetwork.PlayerList[i].NickName;
				localPlayerName[1].text = PhotonNetwork.PlayerList[i].NickName;
			}
			else
			{
				enemyPlayerName[0].text = PhotonNetwork.PlayerList[i].NickName;
				enemyPlayerName[1].text = PhotonNetwork.PlayerList[i].NickName;
			}
		}

		startUI.GetComponent<Animator>().SetTrigger("start");

		yield return new WaitForSeconds(3);

		//deactivate after the animation has finished
		startUI.SetActive(false);
	}

	public void OnGameStart()
	{
		StartCoroutine(ShowStartUI());
	}

}
