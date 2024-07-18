using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Managers;
using Nakama;
using TMPro;
using UnityEngine;

namespace BloodField.Network
{
    public class MatchManager : MonoBehaviour
    {
        public List<string> Players { get; private set; }

        private string _playerName;

        #region Gets/Sets
        public void SetPlayerName(string name) => _playerName = name;
        #endregion

        public async void FindMatch()
        {
            await GameManager.Instance.networkManager.FindMatch(_playerName);
            GameManager.Instance.eventManager.DisplayMessageFindMatchHUDEvent("Searching...");
        }

        public async void Logoff() => await GameManager.Instance.networkManager.Logoff();

        #region Network Events
        public void OnReceivedMatchmakerMatched(IMatchmakerMatched matchmaker)
        {
            Players = matchmaker.Users.Select(s => s.Presence.Username).ToList();

            GameManager.Instance.eventManager.DisplayMessageFindMatchHUDEvent("Found Match!");
            GameManager.Instance.eventManager.DisplayPlayersOnLobbyHUDEvent(Players);

            if (matchmaker.Users.First().Presence.UserId == GameManager.Instance.UserId)
                GameManager.Instance.setIsHost(true);

            GameManager.Instance.gamePlayManager.StartGame();
        }
        #endregion

        void Start()
        {
            GameManager.Instance.eventManager.OnReceivedMatchmakerMatched += OnReceivedMatchmakerMatched;
        }
    }
}