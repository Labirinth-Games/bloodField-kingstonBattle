using UnityEngine;

namespace BloodField.Managers
{
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject ScreenLobby;
        [SerializeField] private GameObject ScreenGame;
        [SerializeField] private GameObject ScreenGameLose;
        [SerializeField] private GameObject ScreenGameWin;
        [SerializeField] private GameObject ScreenLoad;

        private GameObject _lastInstance;

        private void Open(GameObject screen)
        {
            var parent = GameObject.FindGameObjectWithTag("Canvas");

            Clear();

            _lastInstance = Instantiate(screen);
            _lastInstance.transform.position = new Vector3(1920/2, 1080/2, 0);
            _lastInstance.transform.SetParent(parent.transform);
        }

        public void Clear() => Destroy(_lastInstance);

        public void GameScreenShow() => Open(ScreenGame);
        public void LobbyScreenShow() => Open(ScreenLobby);

        #region Events
        private void OnGameLose() => Open(ScreenGameLose);
        private void OnGameWin() => Open(ScreenGameWin);
        #endregion

        void Start()
        {
            GameManager.Instance.eventManager.OnEndGame += (bool isWin) =>
            {
                if (isWin) OnGameWin();
                else OnGameLose();
            };
        }
    }
}