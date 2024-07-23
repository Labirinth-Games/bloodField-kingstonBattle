using BloodField.DTO;
using BloodField.Enums;
using BloodField.Helpers;
using BloodField.Network.Entities;
using Enums;
using Nakama;
using UnityEngine;

namespace BloodField.Managers
{
    public class TurnPreparationManager : MonoBehaviour
    {
        private PlayerMatchDTO _player;

        public bool CanPlayCardPrepadation() => _player?.amountUsedCards < GameManager.Instance.gameSettings.amountDrawCardOnPreparation;

        public void EndTurnPreparation()
        {
            _player.turnStage = PhaseEnum.Main;
            _player.isFinishPreparation = true;
            _player.Reset();

            GameManager.Instance.eventManager.FinishedPreparationPhaseEvent();
        }

        #region Events
        private void OnCardUsed()
        {
            if (_player.isFinishPreparation || GameManager.Instance.matchManager.IsMainPhase()) return;

            _player.amountUsedCards++;

            if (!CanPlayCardPrepadation()) EndTurnPreparation();
        }
        #endregion

        public async void Load()
        {
            _player = GameManager.Instance.matchManager.Players.Find(f => f.userId == GameManager.Instance.UserId);

            await GameManager.Instance.deckManager.Draw(GameManager.Instance.gameSettings.initialAmountInHand);

            GameManager.Instance.player.Load();
            
            GameManager.Instance.eventManager.StartPreparationPhaseEvent();
            GameManager.Instance.eventManager.OnCardUsed += OnCardUsed;
        }
    }
}