using BloodField.DTO;
using BloodField.Enums;
using BloodField.Helpers;
using BloodField.Network.Entities;
using Nakama;
using UnityEngine;

namespace BloodField.Managers
{
    public class TurnManager : MonoBehaviour
    {
        private PlayerMatchDTO _player;

        #region Gets/Sets
        public bool IsMyTurn() => _player.isMyTurn;
        public bool CanPlayCard() => _player.amountUsedCards < GameManager.Instance.MatchSettings.amountDrawCardOnGameplay && IsMyTurn();
        public bool HasMiniatureToPlay() => !_player.isAllMiniatureFinishActions;

        private void OnCardUsed()
        {
            if (GameManager.Instance.matchManager.IsPreparationPhase()) return;

            _player.amountUsedCards++;
            AutomaticEndTurn();
        }
        public void SetMiniatureFinishAction()
        {
            _player.isAllMiniatureFinishActions = GameManager.Instance.miniatureManager.IsAllMiniaturesFinishAction();

            AutomaticEndTurn();
        }
        #endregion

        private void AutomaticEndTurn()
        {
            if (!CanPlayCard() && !HasMiniatureToPlay()) EndTurn();
        }

        public async void EndTurn()
        {
            if (IsMyTurn())
            {
                _player.isMyTurn = false;

                await NetworkHelper.Send<TurnNetworkEntity>(
                    OpCodeEnum.NEW_TURN,
                    new TurnNetworkEntity() { }
                );
            }
        }

        #region Network Events
        private void OnReceiveMatchState(IMatchState matchState)
        {
            NetworkHelper.Listen<TurnNetworkEntity>(matchState, OpCodeEnum.NEW_TURN, (content, isHost, isOwner) =>
            {
                if (isOwner) return;

                _player.Reset();
                _player.isMyTurn = true;

                GameManager.Instance.eventManager.StartMyTurnEvent();
            });
        }
        #endregion

        public void Load(PlayerMatchDTO myPlayer, PlayerMatchDTO firstPlayer)
        {
            _player = myPlayer;

            if (_player.userId == firstPlayer.userId) _player.isMyTurn = true;

            if (GameManager.Instance.networkManager.Socket is not null)
                GameManager.Instance.networkManager.Socket.ReceivedMatchState += OnReceiveMatchState;

            GameManager.Instance.eventManager.OnCardUsed += OnCardUsed;
        }

        // TODO - remover depois
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                _player.Reset();
                _player.isMyTurn = true;

                GameManager.Instance.eventManager.StartMyTurnEvent();
            }
        }
    }
}
