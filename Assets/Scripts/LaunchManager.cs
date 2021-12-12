using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class LaunchManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject enterGamePanel;
    [SerializeField] private GameObject connectionStatusPanel;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private string gameSceneName;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        enterGamePanel.SetActive(true);
        connectionStatusPanel.SetActive(false);
        lobbyPanel.SetActive(false);
    }

    public void ConnectToPhoton()
    {
        if (PhotonNetwork.IsConnected || string.IsNullOrEmpty(PhotonNetwork.NickName)) return;

        PhotonNetwork.ConnectUsingSettings();
        connectionStatusPanel.SetActive(true);
        enterGamePanel.SetActive(false);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    #region photon CallBacks

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.NickName + " is Connected To Photon");
        lobbyPanel.SetActive(true);
        connectionStatusPanel.SetActive(false);
    }

    public override void OnConnected()
    {
        Debug.Log("Connected To Internet");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log(message);
        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
        lobbyPanel.SetActive(false);
        PhotonNetwork.LoadLevel(gameSceneName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " PlayerCount: " +
                  PhotonNetwork.CurrentRoom.PlayerCount);
    }

    #endregion

    #region PrivateMethods

    private void CreateAndJoinRoom()
    {
        var randomRoomName = "Room " + Random.Range(0, 1000);
        var roomOptions = new RoomOptions
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 20
        };
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    #endregion
}