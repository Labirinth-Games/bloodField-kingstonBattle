using BloodField.DTO;
using BloodField.Types;
using BloodField.Helpers;
using BloodField.Network.Entities;
using Nakama;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tiles;
using Render;

namespace BloodField.Managers
{
    public class MatchManager : MonoBehaviour
    {
        public List<PlayerMatchDTO> Players { get; private set; } = new List<PlayerMatchDTO>();
        public PhaseType MatchPhase { get; private set; }
        public string MatchId { get; private set; }
        public bool IsFinishGame { get; private set; } = false;

        private BloodField.Miniatures.Terrain _terrainPermanent = null;

        #region Gets/Sets
        public void SetIsFinishGame(bool val) => IsFinishGame = val;
        public bool IsPreparationPhase() => MatchPhase == PhaseType.Preparation;
        public bool IsMainPhase() => MatchPhase == PhaseType.Main;
        public bool IsReadyPreparationPhasePlayer() => Players.Exists(e => e.isFinishPreparation && e.userId == GameManager.Instance.UserId);

        public bool CanPlayCard()
        {
            if (IsFinishGame) return false;

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

        #region Terrain
        public void ChangeTerrainPermanent(BloodField.Miniatures.Terrain terrain)
        {
            if (_terrainPermanent is not null) _terrainPermanent.Remove();

            _terrainPermanent = terrain;
        }

        public void RemoveTerrainPermanent()
        {
            if (_terrainPermanent is not null) _terrainPermanent.Remove();

            _terrainPermanent = null;
        }
        #endregion

        #region Network Events
        private void OnReceiveMatchState(IMatchState matchState)
        {
            NetworkHelper.Listen<TurnNetworkEntity>(matchState, OpCodeType.TURN_PHASE_PREPARATION_READY, (content, isHost, isOwner) =>
            {
                if (isOwner) return;

                var index = Players.FindIndex(f => f.userId == content.userId);
                Players[index].isFinishPreparation = true;
                Players[index].turnStage = PhaseType.Main;

                ValidateAllPlayerFinishPreparationStage();
            });

            NetworkHelper.Listen<MatchNetworkEntity>(matchState, OpCodeType.MATCH_LOAD, (content, isHost, isOwner) =>
            {
                if (isOwner) return;

                Players = content.Players.Select(userId => new PlayerMatchDTO() { userId = userId }).ToList();

                GameManager.Instance.screenManager.GameScreenShow();
                GameManager.Instance.mapManager.Load();
                GameManager.Instance.cameraControl.Center();

                GameManager.Instance.turnPreparationManager.Load();

                var myPlayer = Players.Find(f => f.userId == GameManager.Instance.UserId);
                GameManager.Instance.turnManager.Load(myPlayer, Players.First());

                MatchPhase = PhaseType.Preparation;
            });

            NetworkHelper.Listen<MatchNetworkEntity>(matchState, OpCodeType.MATCH_STATE, (content, isHost, isOwner) =>
            {
                if (isHost) return;

                if (content.matchState == PhaseType.Main)
                {
                    MatchPhase = PhaseType.Main;
                    GameManager.Instance.eventManager.StartMainPhase();
                }
            });

            NetworkHelper.Listen<MatchNetworkEntity>(matchState, OpCodeType.MATCH_FINISH, (content, isHost, isOwner) =>
            {
                if (isOwner) return;

                GameManager.Instance.eventManager.EndGameEvent(true);
            });

            NetworkHelper.Listen<MiniatureNetworkEntity>(matchState, OpCodeType.MINIATURE_CREATE, (content, isHost, isOwner) =>
            {
                if (isOwner) return;

                MiniatureRender.SpawnRemote(content.id, content.GetCard(), content.GetPosition());
            });
        }
        #endregion

        #region Validations
        public async void ValidateAllPlayerFinishPreparationStage()
        {
            if (!Players.All(p => p.isFinishPreparation)) return;

            MatchPhase = PhaseType.Main;
            await NetworkHelper.Send<MatchNetworkEntity>(OpCodeType.MATCH_STATE, new MatchNetworkEntity() { matchState = PhaseType.Main });

            GameManager.Instance.eventManager.StartMainPhase();
        }
        #endregion

        #region Events
        private async void OnFinishedPreparationPhase()
        {
            if (GameManager.Instance.IsHost) ValidateAllPlayerFinishPreparationStage();
            else await NetworkHelper.Send<TurnNetworkEntity>(OpCodeType.TURN_PHASE_PREPARATION_READY, new TurnNetworkEntity() { });
        }

        private async void OnMiniatureCreated(string id, CardSO card, Tile tile)
        {
            await NetworkHelper.Send<MiniatureNetworkEntity>(OpCodeType.MINIATURE_CREATE, new MiniatureNetworkEntity()
            {
                id = id,
                cardPath = $"Cards/{card.type}/{card.name}",
                y = tile.position.y,
                x = tile.position.x,
            });
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
            MatchPhase = PhaseType.Preparation;

            await NetworkHelper.Send<MatchNetworkEntity>(OpCodeType.MATCH_LOAD, new MatchNetworkEntity() { Players = Players.Select(s => s.userId).ToList() });
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
            GameManager.Instance.eventManager.OnMiniatureCreated += OnMiniatureCreated;
        }

        void OnDestroy()
        {
            GameManager.Instance.eventManager.OnFinishedPreparationPhase -= OnFinishedPreparationPhase;
            GameManager.Instance.eventManager.OnMiniatureCreated -= OnMiniatureCreated;
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
