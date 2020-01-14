using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.VR;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks {


    #region Private Serializable Fields

    /// <summary>
    /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    /// </summary>
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 2;

    #endregion


    #region Public Fields

    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;
    [Tooltip("The UI Panel of the Lobby")]
    [SerializeField]
    private GameObject lobbyPanel;
    [Tooltip("UI Text for Player Name display")]
    [SerializeField]
    private GameObject playerNames;

    public GameObject toggleTutorial;

    #endregion

    #region Private Fields


    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    string gameVersion = "1";
    bool isConnecting;

    #endregion

    #region MonoBehaviour CallBacks


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        //Use to automatically connect to a room when starting the scene
        //Connect();
        //UnityEngine.XR.XRSettings.enabled = false;
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        lobbyPanel.SetActive(false);
    }


    #endregion


    #region Public Methods


    /// <summary>
    /// Start the connection process.
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        isConnecting = true;
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public void ShowPlayerNamesInLobby()
    {
        string players = "";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            players += player.NickName + "\n";
        }
        playerNames.GetComponent<TextMeshProUGUI>().text = players;
    }

    public void StartGame()
    {
        //photonView.RPC("RPCLoadLevel", RpcTarget.MasterClient);
        photonView.RPC("RPCLoadLevelAll", RpcTarget.All);
    }

    public void ToggleTutorial()
    {
        photonView.RPC("RPCToggleTutorial", RpcTarget.Others);
    }

    #endregion


    #region MonoBehaviourPunCallbacks Callbacks


    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
      if(isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        
        ShowPlayerNamesInLobby();
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
    }
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        ShowPlayerNamesInLobby();

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


           // LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        ShowPlayerNamesInLobby();

    }



    #endregion

    #region RPCs
        
    [PunRPC]
    void RPCLoadLevel()
    {
        Debug.Log("MASTER RECIEVED RPC to Load level");
        PhotonNetwork.LoadLevel(1);
    }

    [PunRPC]
    void RPCLoadLevelAll()
    {
        if(toggleTutorial.GetComponent<Toggle>().isOn)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }

    [PunRPC]
    void RPCToggleTutorial()
    {
        toggleTutorial.GetComponent<Toggle>().isOn = !toggleTutorial.GetComponent<Toggle>().isOn;
    }

    #endregion

}
