using System.Collections.Generic;
using System.Linq;
using BloodField.DTO;
using Nakama;
using UnityEngine;

namespace BloodField.Managers
{
    public class LobbyManager : MonoBehaviour
    {
        private List<string> _players;
        private string _playerName;
        private string _matchId;

        #region Gets/Sets
        public void SetPlayerName(string name) => _playerName = name;
        #endregion

        public async void FindMatch()
        {
            await GameManager.Instance.networkManager.FindMatch();
            GameManager.Instance.eventManager.ShowScreenLoadHUDEvent("Searching...");
        }

        public async void Logoff() => await GameManager.Instance.networkManager.Logoff();

        // metodo para poder skipar a connection remover depois
        // simulando que é apenas host steps
        public void SkipNetwork()
        {
            GameManager.Instance.SetIsHost(true);
            GameManager.Instance.SetUserId("00000000-0000-0000-0000-000000000001");

            GameManager.Instance.matchManager.InitialPhase("00000000-0000-0000-0000-000000000000", new List<PlayerMatchDTO>() {
                new PlayerMatchDTO {userId = "00000000-0000-0000-0000-000000000001"},
                new PlayerMatchDTO {userId = "00000000-0000-0000-0000-000000000002"}
            });
        }

        #region Network Events
        public async void OnReceivedMatchmakerMatched(IMatchmakerMatched matchmaker)
        {
            await GameManager.Instance.networkManager.Socket.JoinMatchAsync(matchmaker);

            var accounts = await GameManager.Instance.networkManager.Client.GetUsersAsync(
                GameManager.Instance.networkManager.Session,
                matchmaker.Users.Select(s => s.Presence.UserId).ToList()
            );

            _players = accounts.Users.Select(s => s.DisplayName).ToList();

            GameManager.Instance.eventManager.ShowScreenLoadHUDEvent("Found Match!");
            GameManager.Instance.eventManager.DisplayPlayersOnLobbyHUDEvent(_players);

            if (!GameManager.Instance.isDevelopMode)
                if (matchmaker.Users.First().Presence.UserId == GameManager.Instance.UserId)
                    GameManager.Instance.SetIsHost(true);

            var playersGame = matchmaker.Users.Select(s => new PlayerMatchDTO { userId = s.Presence.UserId }).OrderBy(_ => System.Guid.NewGuid()).ToList();

            if (GameManager.Instance.IsHost)
                GameManager.Instance.matchManager.InitialPhase(_matchId, playersGame);
            else
                GameManager.Instance.matchManager.InitialPhaseRemote(_matchId);
        }

        private void OnReceivedMatchPresence(IMatchPresenceEvent match)
        {
            _matchId = match.MatchId;
        }
        #endregion

        void Start()
        {
            GameManager.Instance.eventManager.OnReceivedMatchPresence += OnReceivedMatchPresence;
            GameManager.Instance.eventManager.OnReceivedMatchmakerMatched += OnReceivedMatchmakerMatched;
        }
    }
}