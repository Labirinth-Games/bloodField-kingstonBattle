using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class TurnManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int amountPlayCardCanActive = 1;

        [Header("callback")]
        public UnityEvent OnUseCard;
        public UnityEvent OnDontUseCard;

        private bool _isTurnPlayer = true;
        private int _amountCardUsed = 0;
        private bool _isAllMiniatureFinish = false;

        #region Gets/Sets
        public bool IsMyTurn() => _isTurnPlayer;

        public void SetCardUsed()
        {
            _amountCardUsed++;

            if (_amountCardUsed >= amountPlayCardCanActive)
                OnDontUseCard?.Invoke();

            GameManager.Instance.deckManager.UseCardOnPlayerHand();

            AutomaticEndTurn();
        }
        public void SetMiniatureFinishAction()
        {
            _isAllMiniatureFinish = GameManager.Instance.gamePlayManager.IsAllMiniaturesFinish();

            AutomaticEndTurn();
        }
        #endregion

        private void AutomaticEndTurn()
        {
            bool isAllCardsPlayed = _amountCardUsed >= amountPlayCardCanActive;
            bool isAllMiniaturesHasFinish = _isAllMiniatureFinish;

            if (isAllCardsPlayed && isAllMiniaturesHasFinish) EndTurn();
        }

        public void EndTurn()
        {
            _isTurnPlayer = !_isTurnPlayer;

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
            
            GameManager.Instance.gamePlayManager.MyTurn();
        }

        private void PlayAgain()
        {
            if (!IsMyTurn())
                EndTurn();
        }

        public void Load()
        {

        }
    }
}
