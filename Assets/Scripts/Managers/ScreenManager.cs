using UnityEngine;

namespace BloodField.Managers
{
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject ScreenLobby;
        [SerializeField] private GameObject ScreenGame;

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
    }
}