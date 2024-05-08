using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private ConnectingPanel cp;
    public void Connect()
    {
        cp.ConnectingToServer();
        if (!PhotonNetwork.ConnectUsingSettings())
        {
            Debug.Log("Error al conectar");
            //errorMessage.ShowErrorMessage("ERROR AL CONECTAR CON EL SERVIDOR.\nComprueba tu conexi�n a internet y vuelve a intentarlo");
            //pc.LobbyToMenu();
        }
    }

    public override void OnConnectedToMaster()
    {
        cp.AlreadyConnected();
        if (!PhotonNetwork.JoinLobby())
        {
            Debug.Log("Error al conectar");
            //errorMessage.ShowErrorMessage("ERROR AL CONECTAR CON EL SERVIDOR.\nComprueba tu conexi�n a internet y vuelve a intentarlo");
            //pc.LobbyToMenu();
        }
    }

    public override void OnJoinedLobby()
    {
       // pc.ConnectingServerToLobby();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

}
