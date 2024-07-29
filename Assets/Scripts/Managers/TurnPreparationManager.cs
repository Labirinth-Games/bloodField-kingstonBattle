using BloodField.DTO;
using BloodField.Types;
using BloodField.Helpers;
using BloodField.Network.Entities;
using BloodField.Types;
using Nakama;
using UnityEngine;

namespace BloodField.Managers
{
    public class TurnPreparationManager : MonoBehaviour
    {
        private PlayerMatchDTO _player;

        public bool CanPlayCardPrepadation() => _player?.amountUsedCards < GameManager.Instance.MatchSettings.amountDrawCardOnPreparation;

        public void EndTurnPreparation()
        {
            _player.turnStage = PhaseType.Main;
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

            await GameManager.Instance.deckManager.Draw(GameManager.Instance.MatchSettings.initialAmountInHand);

            GameManager.Instance.player.Load();
            
            GameManager.Instance.eventManager.StartPreparationPhaseEvent();
            GameManager.Instance.eventManager.OnCardUsed += OnCardUsed;
        }
    }
}