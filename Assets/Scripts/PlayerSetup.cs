using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI playerNameTxt;

    // Start is called before the first frame update
    public void Start()
    {
        Debug.Log("enter");
        if (photonView.IsMine)
        {
            Debug.Log("entertrue");
            GetComponent<TouchAndGo>().enabled = true;
            // GetComponent<PlayerControl>().enabled = true;
        }
        else
        {
            Debug.Log("enterfalse");
            GetComponent<TouchAndGo>().enabled = false;
            // GetComponent<PlayerControl>().enabled = false;
        }

        SetPlayerUi();
    }


    private void SetPlayerUi()
    {
        playerNameTxt.text = photonView.Owner.NickName;
    }
}