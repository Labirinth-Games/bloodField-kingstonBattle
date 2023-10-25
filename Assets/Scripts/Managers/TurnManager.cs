using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class TurnManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private TurnStageEnum turnStage;
        
        [Header("callback")]
        public UnityEvent OnUseCard;
        public UnityEvent OnDontUseCard;
        public UnityEvent OnStartTurnPlayer;

        private bool _isTurnPlayer = true;
        private int _amountCardUsed = 0;
        private bool _isAllMiniatureFinish = false;
        private Dictionary<TurnStageEnum, Func<bool>> _rules;

        #region Gets/Sets
        public bool IsMyTurn() => _isTurnPlayer;
                public TurnStageEnum GetTurnState() => turnStage;
        public bool IsTurnPreparation() => turnStage == TurnStageEnum.Preparation;

        public void SetCardUsed()
        {
            _amountCardUsed++;

            if (_rules[turnStage]())
                OnDontUseCard?.Invoke();

            GameManager.Instance.deckManager.UseCardOnPlayerHand();

            AutomaticEndTurn();
        }
        public void SetMiniatureFinishAction()
        {
            _isAllMiniatureFinish = GameManager.Instance.miniatureManager.IsAllMiniaturesFinish();

            AutomaticEndTurn();
        }
        #endregion

        private void AutomaticEndTurn()
        {
            bool isAllCardsPlayed = _amountCardUsed >= GameManager.Instance.gameSettings.amountPlayCardOnGameplay;
            bool isAllMiniaturesHasFinish = _isAllMiniatureFinish;

            if (isAllCardsPlayed && isAllMiniaturesHasFinish) EndTurn();
        }

        public void EndTurn()
        {
            _isTurnPlayer = !_isTurnPlayer;
            
            // when finish the preparation step
            if (turnStage == TurnStageEnum.Preparation)
                turnStage = TurnStageEnum.GamePlay;

            if (IsMyTurn())
            {
                ResetConfig();
            }

            Invoke(nameof(PlayAgain), 2f);
        }

        private void ResetConfig()
        {
            _amountCardUsed = 0;
            _isAllMiniatureFinish = false;

            OnUseCard?.Invoke();
            OnStartTurnPlayer?.Invoke();
        }

        private void PlayAgain()
        {
            if (!IsMyTurn())
                EndTurn();
        }

        public void Load()
        {
            turnStage = TurnStageEnum.Preparation;

            _rules = new Dictionary<TurnStageEnum, Func<bool>>
            {
                { TurnStageEnum.Preparation, () => _amountCardUsed >= GameManager.Instance.gameSettings.amountPlayCardOnPreparation },
                { TurnStageEnum.GamePlay, () => _amountCardUsed >= GameManager.Instance.gameSettings.amountPlayCardOnGameplay }
            };
        }
    }
}
