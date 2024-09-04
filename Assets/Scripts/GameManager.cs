using System.Threading.Tasks;
using BloodField.HUD;
using BloodField.Managers;
using BloodField.Network;
using Controls;
using Helpers;
using Render;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodField.Managers
{
    public class GameManager : Utils.Singleton<GameManager>
    {
        [Header("Settings")]
        public MatchConfigSO MatchSettings;
        public GameConfigSO GameSettings;
        public bool isDebug = false;
        public bool isDevelopMode = false;

        [Header("References")]
        public MapManager mapManager;
        public TurnManager turnManager;
        public TurnPreparationManager turnPreparationManager;
        public CameraControl cameraControl;
        public CardManager cardManager;
        public DeckManager deckManager;
        public MatchManager matchManager;
        public MiniatureManager miniatureManager;
        public Player player;
        public MiniatureMouseHelper miniatureMouseHelper;
        public NetworkManager networkManager;
        public EventManager eventManager;
        public LobbyManager lobbyManager;
        public ScreenManager screenManager;
        public LogHUD logHUD;

        public string UserId { get; private set; }
        public bool IsLocal { get; private set; } = false;
        public bool IsHost { get; private set; } = false;
        public bool IsFinishGame { get; private set; } = false;

        #region Gets/Sets
        public void SetUserId(string userId) => UserId = userId;
        public void SetIsLocal(bool value) => IsLocal = value;
        public void SetIsHost(bool value) => IsHost = value;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        async void Update()
        {
            if (Input.GetKeyDown(KeyCode.L) && isDevelopMode)
            {
                await networkManager.Logoff();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
            }
        }

        async void Start()
        {
            await Task.Delay(1000);

            var client = await networkManager.Authenticator();

            if (string.IsNullOrEmpty(client.User.DisplayName)) SceneManager.LoadScene("RegisterScene");
            else
            {
                SceneManager.LoadScene("GameScene");
                await Task.Delay(500);
                screenManager.LobbyScreenShow();
            }
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

            if (TryGetComponent(out MiniatureMouseHelper miniatureMouseHelper))
                this.miniatureMouseHelper = miniatureMouseHelper;

            if (TryGetComponent(out MiniatureManager miniatureManager))
                this.miniatureManager = miniatureManager;

            if (TryGetComponent(out NetworkManager networkManager))
                this.networkManager = networkManager;
        }
    }
}
