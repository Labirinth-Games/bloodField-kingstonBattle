using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Network
{
    public class LobbyNetworkManager : NetworkManager
    {
        [Header("References")]
        [SerializeField] private LobbyPlayer lobbyPlayerPrefab;
        [SerializeField] private GameObject lobbyDisplayNames;
        [SerializeField] private NetworkHud networkHud;
        [SerializeField] private GamePlayer gamePlayerPrefab;

        [Header("Settings")]
        [SerializeField] private int maxPlayers = 2;
        [SerializeField] private int minPlayers = 2;
        [SerializeField] private DiscoveryNetworkManager networkDiscoveryManager;

        private List<LobbyPlayer> _lobbyPlayers = new List<LobbyPlayer>();
        private List<GamePlayer> _playersInGame = new List<GamePlayer>();

        private const string _lobbyScene = "LobbyScene";

        #region In Game
        public void InitHost()
        {
            networkHud.ShowStart();
            StartHost();
            networkDiscoveryManager.AdvertiseServer();
        }

        public void InitClient()
        {
            networkHud.WaitHost();
            base.networkAddress = "127.0.0.1";
            base.StartClient();
        }
        #endregion

        public override void OnStartServer()
        {
            base.OnStartServer();

            NetworkServer.RegisterHandler<KingClientMessage>(OnKingClientAddPlayer);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            
            string playerName = "Player" + UnityEngine.Random.Range(1000, 10000).ToString();
            KingClientMessage message = new KingClientMessage
            {
                name = playerName
            };

            NetworkClient.Send(message);

            if(!lobbyDisplayNames.activeSelf)
                lobbyDisplayNames.SetActive(true);
        }

        public override void OnStartClient()
        {
            List<GameObject> spawnablePrefabs = Resources.LoadAll<GameObject>("Prefabs").ToList();

            foreach (GameObject prefab in spawnablePrefabs)
            {
                NetworkClient.RegisterPrefab(prefab);
                Debug.Log("Registering prefab: " + prefab);
            }
        }

        public void OnKingClientAddPlayer(NetworkConnectionToClient conn, KingClientMessage message)
        {
            if (SceneManager.GetActiveScene().name == _lobbyScene)
            {
                LobbyPlayer lobbyPlayer = Instantiate(lobbyPlayerPrefab);
                lobbyPlayer.displayName = message.name;

                lobbyPlayer.connectionId = conn.connectionId;

                NetworkServer.AddPlayerForConnection(conn, lobbyPlayer.gameObject);

                _lobbyPlayers.Add(lobbyPlayer);

                Debug.Log("Player connection id: " + lobbyPlayer.connectionId.ToString());
            }
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            Debug.Log($"Player {conn.connectionId} Connecting to server...");

            if (numPlayers > maxPlayers)
            {
                Debug.Log("there are a lot of players, a crowded room!");
                conn.Disconnect();
            }
        }

        public void StartGame()
        {
            if (CanStartGame() && SceneManager.GetActiveScene().name == _lobbyScene)
            {
                ServerChangeScene("GameScene");
            }
        }

        public void Disconnect()
        {
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                ServerChangeScene(_lobbyScene);
            }
        }

        // public void FinishGame()
        // {
        //     if (SceneManager.GetActiveScene().name == gameManager.mapSelected)
        //     {
        //         ServerChangeScene(_lobbyScene);
        //         gameManager.menuManager.Active(EnumMenuType.Room);
        //     }
        // }

        private bool CanStartGame()
        {
            // var players = GameObject.FindGameObjectsWithTag("Player Lobby");

            // foreach (var player in players)
            // {
            //     if (!player.GetComponent<LobbyPlayer>().GetIsReady())
            //         return false;
            // }

            return true;
        }

        public override void OnServerChangeScene(string newSceneName)
        {
            // load the list player on lobby
            if (SceneManager.GetActiveScene().name == "lobbyScene" && newSceneName == _lobbyScene)
            {
                List<GamePlayer> players = new List<GamePlayer>(_playersInGame);
                _playersInGame.Clear();

                foreach (var player in players)
                {
                    var conn = player.connectionToClient;
                    var instance = Instantiate(lobbyPlayerPrefab);

                    instance.displayName = player.displayName;
                    instance.connectionId = player.connectionId;

                    _lobbyPlayers.Add(instance);

                    NetworkServer.Destroy(conn.identity.gameObject);
                    NetworkServer.ReplacePlayerForConnection(conn, instance.gameObject, true);
                    Debug.Log("Spawned new GamePlayer.");
                }

            }

            // create all players on game based in lobby list
            if (SceneManager.GetActiveScene().name == _lobbyScene && newSceneName == "GameScene")
            {
                List<LobbyPlayer> players = new List<LobbyPlayer>(_lobbyPlayers);
                players.Reverse();

                foreach (var player in players)
                {
                    var conn = player.connectionToClient;
                    var instance = Instantiate(gamePlayerPrefab);

                    instance.displayName = player.displayName;
                    instance.connectionId = player.connectionId;

                    _playersInGame.Add(instance);

                    NetworkServer.Destroy(conn.identity
                        .gameObject);
                    NetworkServer.ReplacePlayerForConnection(conn, instance.gameObject, true);
                    Debug.Log("Spawned new GamePlayer.");
                }

            }

            base.OnServerChangeScene(newSceneName);
        }
    }

    public struct KingClientMessage : NetworkMessage
    {
        public string name;
    }

}