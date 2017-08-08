using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviour {

    public static PlayerNetwork instance;
    public string PlayerName { get; private set; }
    private PhotonView _PhotonView;
    private int PlayerInGames = 0;

    private void Awake()
    {
        instance = this;
        _PhotonView = GetComponent<PhotonView>();

        PlayerName = "Distule#" + Random.Range(1000, 9999);
        PhotonNetwork.playerName = PlayerName;
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
        PlayerInGames = 1;

        _PhotonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others);
    }

    private void NonMasterLoadedGame()
    {
        _PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient);
    }

    [PunRPC]
    private void RPC_LoadGameOthers()
    {
        PhotonNetwork.LoadLevel(1);
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        PlayerInGames++;

        if (PlayerInGames == PhotonNetwork.playerList.Length)
        {
            print("All players are in the game scene");
        }
    }
}
