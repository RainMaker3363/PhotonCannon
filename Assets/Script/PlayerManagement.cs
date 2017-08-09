using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour {

    public static PlayerManagement Instance;
    private PhotonView _PhotonView;

    private List<PlayerStats> PlayerStats = new List<global::PlayerStats>();


    private void Awake()
    {
        Instance = this;

        _PhotonView = GetComponent<PhotonView>();
    }

    public void AddPlayerStats(PhotonPlayer photonPlayer)
    {

        int index = PlayerStats.FindIndex(x => x._photonPlayer == photonPlayer);

        if(index == -1)
        {
            PlayerStats.Add(new global::PlayerStats(photonPlayer, 30));
        }
    }

    public void ModifiyHealth(PhotonPlayer photonPlayer, int value)
    {
        int index = PlayerStats.FindIndex(x => x._photonPlayer == photonPlayer);

        if(index == -1)
        {
            PlayerStats playerStats = PlayerStats[index];
            playerStats._Health += value;
            PlayerNetwork.instance.NewHealth(photonPlayer, playerStats._Health);
        }
    }

}

public class PlayerStats
{
    public PlayerStats(PhotonPlayer photonPlayer, int health)
    {
        _photonPlayer = photonPlayer;
        _Health = health;
    }

    public readonly PhotonPlayer _photonPlayer;
    public int _Health;
}
