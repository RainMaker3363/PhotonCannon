using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerNetwork : MonoBehaviour {

    public static PlayerNetwork instance;
    public string PlayerName { get; private set; }
    private PhotonView _PhotonView;
    private int PlayerInGames = 0;

    private PlayerMovement CurrnetPlayer;

    private void Awake()
    {
        instance = this;
        _PhotonView = GetComponent<PhotonView>();

        PlayerName = "Distule#" + UnityEngine.Random.Range(1000, 9999);
        PhotonNetwork.playerName = PlayerName;
        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 30;

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Game")
        {
            if(PhotonNetwork.isMasterClient)
            {
                MasterLoadedGame();
            }
            else
            {
                NonMasterLoadedGame();
            }
        }
    }

    private void MasterLoadedGame()
    {
        _PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
        _PhotonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others);
    }

    private void NonMasterLoadedGame()
    {
        _PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
    }

    [PunRPC]
    private void RPC_LoadGameOthers()
    {
        PhotonNetwork.LoadLevel(1);
    }

    [PunRPC]
    private void RPC_LoadedGameScene(PhotonPlayer photonPlayer)
    {
        PlayerManagement.Instance.AddPlayerStats(photonPlayer);

        PlayerInGames++;

        if (PlayerInGames == PhotonNetwork.playerList.Length)
        {
            print("All players are in the game scene");
            _PhotonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
        }
    }

    public void NewHealth(PhotonPlayer photonPlayer, int health)
    {
        _PhotonView.RPC("RPC_NewHealth", photonPlayer, health);
    }

    [PunRPC]
    private void RPC_NewHealth(int heath)
    {
        if(CurrnetPlayer == null)
        {
            return;
        }

        if(heath <= 0)
        {
            PhotonNetwork.Destroy(CurrnetPlayer.gameObject);
        }
        else
        {
            CurrnetPlayer.Health -= heath;
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        float randomValue = UnityEngine.Random.Range(-5f, 5f);
        
        GameObject obj = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "NewPlayer"), Vector3.up * randomValue, Quaternion.identity, 0);
        CurrnetPlayer = obj.GetComponent<PlayerMovement>();
    }
}
