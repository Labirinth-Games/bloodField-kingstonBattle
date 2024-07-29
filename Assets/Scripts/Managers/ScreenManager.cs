using UnityEngine;

namespace BloodField.Managers
{
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject ScreenLobby;
        [SerializeField] private GameObject ScreenGame;
        [SerializeField] private GameObject ScreenGameLose;
        [SerializeField] private GameObject ScreenGameWin;

        public void GameScreenShow()
        {
            ScreenGame.SetActive(true);
            ScreenLobby.SetActive(false);
        }
        public void LobbyScreenShow()
        {
            ScreenLobby.SetActive(true);
            ScreenGame.SetActive(false);
        }

        #region Events
        private void OnGameLose()
        {
            ScreenGameLose.SetActive(true);
        }
        private void OnGameWin()
        {
            ScreenGameWin.SetActive(true);
        }
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