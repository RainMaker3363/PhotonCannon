using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour {

    [SerializeField]
    private Text _roomName;
    private Text RoomName
    {
        get
        {
            return _roomName;
        }
    }

    public void OnClick_CreateRoom()
    {
        RoomOptions roomOption = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 4 };
        if(PhotonNetwork.CreateRoom(RoomName.text, roomOption, TypedLobby.Default))
        {
            print("Create room Successfully sent");
        }
        else
        {
            print("Create room Failed to send");
        }
    }

    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        print("Create Room Failed: " + codeAndMessage[1]);
    }

    private void OnCreatedRoom()
    {
        print("Room Created successfully.");
    }
}
