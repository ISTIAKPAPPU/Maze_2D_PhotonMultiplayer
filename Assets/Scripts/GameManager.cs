using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject pfPlayer;
    [SerializeField] private GameObject timer;
    [SerializeField] private string lunchSceneName = "GameLauncherScene";
    public static GameManager Instance;
    public Transform playerParentTransform;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (pfPlayer != null)
            {
                Debug.Log("Players:" + PhotonNetwork.CurrentRoom.PlayerCount);
                if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
                {
                    Init();
                }

                var randomPos = (PhotonNetwork.CurrentRoom.PlayerCount == 1)
                    ? playerParentTransform.GetChild(0).transform.position
                    : playerParentTransform.GetChild(1).transform.position;
                var playerObj = PhotonNetwork.Instantiate(pfPlayer.name, randomPos,
                    Quaternion.identity);
            }
        }
    }

    private void Init()
    {
        GameValues.IsGameOver = false;
        GameValues.IsGameStart = false;
        GameValues.IsPlayerWin = false;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            StartCoroutine(StartGame());
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " PlayerCount: " +
                  PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame()
    {
        timer.SetActive(true);
        yield return new WaitForSeconds(5f);
        GameValues.IsGameStart = true;
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(lunchSceneName);
    }
}