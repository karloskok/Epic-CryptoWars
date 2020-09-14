using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyButtons : MonoBehaviour
{
    public Button back;
    // Start is called before the first frame update


    public void BackToMenu()
    {
        PunNetworkManager punNetworkManager = GameObject.FindObjectOfType<PunNetworkManager>();
        punNetworkManager.BackToMenuFromLobby();
    }

}
