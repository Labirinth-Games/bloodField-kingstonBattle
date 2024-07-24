using BloodField.DTO;
using BloodField.Enums;
using BloodField.Helpers;
using BloodField.Network.Entities;
using Enums;
using Miniatures;
using Nakama;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BloodField.Managers
{
    public class MatchManager : MonoBehaviour
    {
        public List<PlayerMatchDTO> Players { get; private set; } = new List<PlayerMatchDTO>();
        public PhaseEnum MatchPhase { get; private set; }
        public string MatchId { get; private set; }

        #region Gets/Sets
        public bool IsPreparationPhase() => MatchPhase == PhaseEnum.Preparation;
        public bool IsMainPhase() => MatchPhase == PhaseEnum.Main;
        public bool IsReadyPreparationPhasePlayer() => Players.Exists(e => e.isFinishPreparation && e.userId == GameManager.Instance.UserId);

        public bool CanPlayCard()
        {
            if (IsPreparationPhase() && GameManager.Instance.matchManager.IsReadyPreparationPhasePlayer()) return false;
            if (IsMainPhase() && !GameManager.Instance.turnManager.IsMyTurn()) return false;
            if (IsMainPhase() && !GameManager.Instance.turnManager.CanPlayCard()) return false;

            return true;
        }

        public bool CanActionMiniature()
        {
            if (IsPreparationPhase() && GameManager.Instance.matchManager.IsReadyPreparationPhasePlayer()) return false;
            if (IsMainPhase() && !GameManager.Instance.turnManager.IsMyTurn()) return false;
            if (IsMainPhase() && !GameManager.Instance.turnManager.HasMiniatureToPlay()) return false;

            return true;
        }
        #endregion

        #region Network Events
        private void OnReceiveMatchState(IMatchState matchState)
        {
            NetworkHelper.Listen<TurnNetworkEntity>(matchState, OpCodeEnum.TURN_PHASE_PREPARATION_READY, (content, isHost, isOwner) =>
            {
                if (isOwner) return;

                var index = Players.FindIndex(f => f.userId == content.userId);
                Players[index].isFinishPreparation = true;
                Players[index].turnStage = PhaseEnum.Main;

                ValidateAllPlayerFinishPreparationStage();
            });

            NetworkHelper.Listen<MatchNetworkEntity>(matchState, OpCodeEnum.MATCH_LOAD, (content, isHost, isOwner) =>
            {
                if (isOwner) return;

                Players = content.Players.Select(userId => new PlayerMatchDTO() { userId = userId }).ToList();

                GameManager.Instance.screenManager.GameScreenShow();
                GameManager.Instance.mapManager.Load();
                GameManager.Instance.cameraControl.Center();

                GameManager.Instance.turnPreparationManager.Load();

                var myPlayer = Players.Find(f => f.userId == GameManager.Instance.UserId);
                GameManager.Instance.turnManager.Load(myPlayer, Players.First());

                MatchPhase = PhaseEnum.Preparation;
            });

            NetworkHelper.Listen<MatchNetworkEntity>(matchState, OpCodeEnum.MATCH_STATE, (content, isHost, isOwner) =>
            {
                if (isHost) return;

                if (content.matchState == PhaseEnum.Main)
                {
                    MatchPhase = PhaseEnum.Main;
                    GameManager.Instance.eventManager.StartMainPhase();
                }
            });
        }
        #endregion

        #region Validations
        public async void ValidateAllPlayerFinishPreparationStage()
        {
            if (!Players.All(p => p.isFinishPreparation)) return;

            MatchPhase = PhaseEnum.Main;
            await NetworkHelper.Send<MatchNetworkEntity>(OpCodeEnum.MATCH_STATE, new MatchNetworkEntity() { matchState = PhaseEnum.Main });

            GameManager.Instance.eventManager.StartMainPhase();
        }
        #endregion

        #region Events
        private async void OnFinishedPreparationPhase()
        {
            if (GameManager.Instance.IsHost) ValidateAllPlayerFinishPreparationStage();
            else await NetworkHelper.Send<TurnNetworkEntity>(OpCodeEnum.TURN_PHASE_PREPARATION_READY, new TurnNetworkEntity() { });
        }
        #endregion

        public async void InitialPhase(string matchId, List<PlayerMatchDTO> players)
        {
            MatchId = matchId;
            Players = players;

            Subscribers();

            GameManager.Instance.deckManager.Load();

            GameManager.Instance.screenManager.GameScreenShow();
            GameManager.Instance.mapManager.Load();
            GameManager.Instance.cameraControl.Center();

            GameManager.Instance.turnPreparationManager.Load();

            var firstPlayer = Players.First();
            var myPlayer = Players.Find(f => f.userId == GameManager.Instance.UserId);

            GameManager.Instance.turnManager.Load(myPlayer, Players.First());
            MatchPhase = PhaseEnum.Preparation;

            await NetworkHelper.Send<MatchNetworkEntity>(OpCodeEnum.MATCH_LOAD, new MatchNetworkEntity() { Players = Players.Select(s => s.userId).ToList() });
        }

        public void InitialPhaseRemote(string matchId)
        {
            MatchId = matchId;

            GameManager.Instance.deckManager.Subscribers();
            Subscribers();
        }

        private void Subscribers()
        {
            if (GameManager.Instance.networkManager.Socket is not null)
                GameManager.Instance.networkManager.Socket.ReceivedMatchState += OnReceiveMatchState;

            GameManager.Instance.eventManager.OnFinishedPreparationPhase += OnFinishedPreparationPhase;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.O)) // TODO - remover depois dos testes
            {
                Players.ForEach(p => p.isFinishPreparation = true);
                ValidateAllPlayerFinishPreparationStage();
            }
        }
    }
}
