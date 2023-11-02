using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class TurnManager : NetworkBehaviour
    {
        [Header("Settings")]
        [SerializeField] private TurnStageEnum turnStage;

        [Header("callback")]
        public UnityEvent OnUseCard;
        public UnityEvent OnDontUseCard;
        public UnityEvent OnStartTurnPlayer;

        [SyncVar] private uint _turnPlayer;
        private List<uint> _players = new List<uint>();
        private int _amountCardUsed = 0;
        private bool _isAllMiniatureFinish = false;
        private Dictionary<TurnStageEnum, Func<bool>> _rules;

        #region Gets/Sets
        public bool IsMyTurn() => _turnPlayer == netId;
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

        private uint NextTurn()
        {
            int next = _players.FindIndex(f => f == _turnPlayer) + 1;

            if (next >= _players.Count)
                return _players[0];

            return _players[next];
        }

        public void EndTurn()
        {
            _turnPlayer = NextTurn();

            // when finish the preparation step
            if (turnStage == TurnStageEnum.Preparation)
                turnStage = TurnStageEnum.GamePlay;

            if (IsMyTurn())
            {
                _amountCardUsed = 0;
                _isAllMiniatureFinish = false;

                OnUseCard?.Invoke();
                OnStartTurnPlayer?.Invoke();
            }
        }

        public void Load()
        {
            turnStage = TurnStageEnum.Preparation;

            _rules = new Dictionary<TurnStageEnum, Func<bool>>
            {
                { TurnStageEnum.Preparation, () => _amountCardUsed >= GameManager.Instance.gameSettings.amountPlayCardOnPreparation },
                { TurnStageEnum.GamePlay, () => _amountCardUsed >= GameManager.Instance.gameSettings.amountPlayCardOnGameplay }
            };

            // add the players on turns
            foreach (Player player in GameManager.Instance.networkManager.playersInGame)
            {
                _players.Add(player.netId);
            }
        }
    }
}
