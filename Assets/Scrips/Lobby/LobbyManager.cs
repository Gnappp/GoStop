using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "1";

    public Text infoText;
    public Button joinButton;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        infoText.text = "Connecting To Server";
    }

    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        infoText.text = "Connected!";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        infoText.text = $"Disconnted : {cause.ToString()}";

        PhotonNetwork.ConnectUsingSettings();
    }
    public void Connect()
    {
        joinButton.interactable = false;

        if(PhotonNetwork.IsConnected)
        {
            infoText.text = "Connecting";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            infoText.text = "Disconnted";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        infoText.text = "Not empty room, create room";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        infoText.text = "Conneted Room";
        PhotonNetwork.LoadLevel("Gostop");
    }
}
