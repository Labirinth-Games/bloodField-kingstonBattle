using BloodField.Managers;
using BloodField.Network;
using Controls;
using Helpers;
using Render;
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
        public NetworkManager networkManager;
        public EventManager eventManager;
        public MatchManager matchManager;

        [Header("Renders")]
        public MiniatureRender miniatureRender;

        public string UserId { get; private set; }
        public bool IsLocal { get; private set; } = false;
        public bool IsHost { get; private set; } = false;

        #region Gets/Sets
        public void setSessionId(string userId) => UserId = userId;
        public void setIsLocal(bool value) => IsLocal = value;
        public void setIsHost(bool value) => IsHost = value;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            networkManager.Connect();
        }

        private void OnValidate()
        {
            if (TryGetComponent(out MapManager mapManager))
                this.mapManager = mapManager;

            if (TryGetComponent(out CameraControl cameraControl))
                this.cameraControl = cameraControl;

            if (TryGetComponent(out TurnManager turnManager))
                this.turnManager = turnManager;

            if (TryGetComponent(out DeckManager deckManager))
                this.deckManager = deckManager;

            if (TryGetComponent(out GamePlayManager gamePlay))
                this.gamePlayManager = gamePlay;

            if (TryGetComponent(out MiniatureMouseHelper miniatureMouseHelper))
                this.miniatureMouseHelper = miniatureMouseHelper;

            if (TryGetComponent(out MiniatureManager miniatureManager))
                this.miniatureManager = miniatureManager;

            if (TryGetComponent(out NetworkManager networkManager))
                this.networkManager = networkManager;
        }
    }
}
