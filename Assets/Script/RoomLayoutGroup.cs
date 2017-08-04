using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutGroup : MonoBehaviour {

    [SerializeField]
    private GameObject _roomListngPrefab;
    private GameObject RoomListingPrefab
    {
        get
        {
            return _roomListngPrefab;
        }
    }

    private List<RoomListing> _roomListingButtons = new List<RoomListing>();
    private List<RoomListing> RoomListingButtons
    {
        get
        {
            return _roomListingButtons;
        }
    }

    private void OnReceivedRoomListUpdate()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();

        foreach(RoomInfo room in rooms)
        {
            RoomReceived(room);
        }

        RemoveOldRooms();
    }

    private void RoomReceived(RoomInfo room)
    {
        int index = RoomListingButtons.FindIndex(x => x.RoomName == room.Name);

        if(index == -1)
        {
            if(room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject roomListingObj = Instantiate(RoomListingPrefab);
                roomListingObj.transform.SetParent(transform, false);

                RoomListing roomListing = roomListingObj.GetComponent<RoomListing>();
                RoomListingButtons.Add(roomListing);

                index = (RoomListingButtons.Count - 1);
            }
        }

        if(index != -1)
        {
            RoomListing roomlisting = RoomListingButtons[index];
            roomlisting.SetRoomNameText(room.Name);
            roomlisting.Updated = true;
        }
    }

    private void RemoveOldRooms()
    {
        List<RoomListing> removeRooms = new List<RoomListing>();

        foreach(RoomListing roomlisting in RoomListingButtons)
        {
            if(!roomlisting.Updated)
            {
                removeRooms.Add(roomlisting);
            }
            else
            {
                roomlisting.Updated = false;
            }
        }

        foreach (RoomListing roomlisting in removeRooms)
        {
            GameObject roomListingObj = roomlisting.gameObject;
            RoomListingButtons.Remove(roomlisting);
            Destroy(roomListingObj);
        }
    }
}
