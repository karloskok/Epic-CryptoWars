using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
using Photon.Realtime;
#endif

public class NetworkRPC
#if PHOTON_UNITY_NETWORKING
 : PunRPC
#else
    : Attribute
#endif
{

}

public class PunNetworkManager
#if PHOTON_UNITY_NETWORKING
 : MonoBehaviourPunCallbacks
#else
   : MonoBehaviour
#endif
{
    PlayerKeysHolder playerKeysHolder;
    public NetworkMessenger Messenger;
    public static PunNetworkManager NetworkManager;
#if PHOTON_UNITY_NETWORKING
    private PhotonView view;
#endif
    public bool IsMaster
    {
        get
        {
#if PHOTON_UNITY_NETWORKING
            return PhotonNetwork.IsMasterClient;
#else
             return false;
#endif
        }
    }
    public int PlayerIndex
    {
        get
        {
#if PHOTON_UNITY_NETWORKING
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[i].UserId)
                {
                    return i;
                }
            }
#endif
            return 0;
        }
    }

    private void Awake()
    {
        if (NetworkManager && NetworkManager != this)
        {
            Destroy(gameObject);
            return;
        }
        if (!NetworkManager)
        {
            NetworkManager = this;
#if PHOTON_UNITY_NETWORKING
            view = gameObject.AddComponent<PhotonView>();
            view.ViewID = 1;
            view.ObservedComponents = new List<Component>(0)
            {
                Messenger
            };
#endif
            DontDestroyOnLoad(gameObject);
        }


        playerKeysHolder = Resources.Load<PlayerKeysHolder>("Data/PlayerData");
        if(playerKeysHolder)
            Debug.Log(playerKeysHolder.Name);
    }

    private IEnumerator Start()
    {
        while (true)
        {
            UpdateConnecting();
            yield return new WaitForSeconds(1.0f);
        }
    }

    //private void Start()
    //{
    //    UpdateConnecting();
    //}

    public void RPCToOthers(string methodName, params object[] parameters)
    {
#if PHOTON_UNITY_NETWORKING
        view.RPC(methodName, RpcTarget.Others, parameters);
#endif
    }

    private void UpdateConnecting()
    {
#if PHOTON_UNITY_NETWORKING
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No internet -> UpdateConnecting() -> LoadLogin");
            //LoadLobby();
            LoadLogin();
        }
        else if (Application.internetReachability != NetworkReachability.NotReachable && !PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
#endif
    }

    public void ClearActions()
    {
        Messenger.CleraActions();

    }

    public void Disconnect()
    {
        //TODO: on disconnect
        Messenger.CleraActions();
#if PHOTON_UNITY_NETWORKING
        PhotonNetwork.Disconnect();
#endif
    }
#if PHOTON_UNITY_NETWORKING
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
       
    }

    
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        //set player nickname
        PhotonNetwork.NickName = playerKeysHolder.Name.ToString();

        //if (!PhotonNetwork.InLobby && SceneManager.GetActiveScene().name != "LobbyScene")
        //{
        //    PhotonNetwork.JoinLobby();
        //}
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby() -> JoinRandomRoom()");
        PhotonNetwork.JoinRandomRoom();
        //Debug.Log("OnJoinedLobby() -> JoinRandomRoom()");

    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string name = "Room_" + UnityEngine.Random.Range(1, 1000);
        PhotonNetwork.JoinOrCreateRoom(name, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
        Debug.Log("OnJoinRandomFailed() Room name: " + name);
    }
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            LoadGame();
        }
    }
    public override void OnCreatedRoom()
    {
        PhotonNetwork.CurrentRoom.IsVisible = true;
        PhotonNetwork.CurrentRoom.IsOpen = true;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom " + newPlayer.NickName);
        LoadGame();
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        string name = "Room_" + UnityEngine.Random.Range(1, 1000);
        PhotonNetwork.JoinOrCreateRoom(name, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
        Debug.Log("OnJoinRoomFailed() Room name: " + name);
       // Debug.Log("OnJoinRoomFailed " + message);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //TODO: open panel disconnected and load menu

        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {

        Messenger.CleraActions();
        //LoadLobby();
        LoadMainMenu();
        Debug.Log("OnLeftRoom()");

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LeaveRoom();
        //LoadLobby();
        //LoadLogin();
        //TODO: disconnect
        Debug.Log("OnDisconnected()");
    }
#endif

    private void LoadGame()
    {
        if (SceneManager.GetActiveScene().name != "GameScene")
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    private void LoadLobby()
    {
        if (SceneManager.GetActiveScene().name != "LobbyScene")
        {
            SceneManager.LoadScene("LobbyScene");
        }
    }

    private void LoadMainMenu()
    {
        if (SceneManager.GetActiveScene().name != "MainMenuScene")
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    private void LoadLogin()
    {
        if (SceneManager.GetActiveScene().name != "LoginScene")
        {
            SceneManager.LoadScene("LoginScene");
        }
    }

    public void BackToMenuFromLobby()
    {
        if (PhotonNetwork.InRoom)
        {
            if(PhotonNetwork.PlayerList.Length < 2)
            {
                //back to menu
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LeaveLobby();
            }
        }
        else if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
    }

    public void BackToMenuFromRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            //back to menu
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
            
        }
        else if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
    }

    //leave the photon room
    public void LeaveRoom()
    {
        if(PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
        if(PhotonNetwork.InLobby)
            PhotonNetwork.LeaveLobby();
        Debug.Log("PhotonNetwork.LeaveRoom()");
    }

    public void Battle()//todo battle
    {
        PhotonNetwork.JoinLobby();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PhotonNetwork.InRoom) { 
                LeaveRoom();
            }

        }
    }


}
