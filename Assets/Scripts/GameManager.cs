using Controls;
using Helpers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class GameManager : Utils.Singleton<GameManager>
    {
        [Header("Settings")]
        public MatchConfigSO gameSettings;
        public bool isDebug = false;

        [Header("References")]
        public MapManager mapManager;
        public TurnManager turnManager;
        public CameraControl cameraControl;
        public CardManager cardManager;
        public DeckManager deckManager;
        public GamePlayManager gamePlayManager;
        public MiniatureManager miniatureManager;
        public Player player;
        public MiniatureMouseHelper miniatureMouseHelper;
        public Network.LobbyNetworkManager networkManager;

        private void Start()
        {
            mapManager.Load();
            cameraControl.Center();

            gamePlayManager.StartMatch();
        }

        private void OnValidate()
        {
            if (TryGetComponent(out MapManager mapManager))
                this.mapManager = mapManager;

            if (TryGetComponent(out CameraControl cameraControl))
                this.cameraControl = cameraControl;

            if (TryGetComponent(out TurnManager matchManager))
                this.turnManager = matchManager;

            if (TryGetComponent(out DeckManager deckManager))
                this.deckManager = deckManager;

            if (TryGetComponent(out GamePlayManager gamePlay))
                this.gamePlayManager = gamePlay;

            if (TryGetComponent(out MiniatureMouseHelper miniatureMouseHelper))
                this.miniatureMouseHelper = miniatureMouseHelper; 
            
            if (TryGetComponent(out MiniatureManager miniatureManager))
                this.miniatureManager = miniatureManager;
        }
    }
}
