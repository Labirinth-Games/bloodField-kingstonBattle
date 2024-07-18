using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public UnityEvent OnStartTurnPlayer;

        private string _turnPlayer;
        private List<bool> _isAllReady = new List<bool>();
        private List<string> _players = new List<string>();

        private int _amountCardUsed = 0;
        private bool _isAllMiniatureFinish = false;
        private Dictionary<TurnStageEnum, Func<bool>> _rules;

        #region Gets/Sets
        public bool IsMyTurn() => _turnPlayer == GameManager.Instance.UserId;
        public bool CanPlayCard() => _rules[turnStage]() && (IsTurnPreparation() || IsMyTurn());
        public TurnStageEnum GetTurnState() => turnStage;
        public bool IsTurnPreparation() => turnStage == TurnStageEnum.Preparation;
        public bool IsAllReadyToInitGame() => _isAllReady.Count == _players.Count;

        public void SetCardUsed()
        {
            _amountCardUsed++;
            GameManager.Instance.deckManager.PlayerUsedACardOnHand();

            AutomaticEndTurn();
        }
        public void SetMiniatureFinishAction()
        {
            _isAllMiniatureFinish = GameManager.Instance.miniatureManager.IsAllMiniaturesFinish();

            AutomaticEndTurn();
        }

        private void Reset()
        {
            _amountCardUsed = 0;
            _isAllMiniatureFinish = false;
        }

        public void SetIsReadyClient() => _isAllReady.Add(true);
        #endregion

        private void AutomaticEndTurn()
        {
            if (CanPlayCard() && _isAllMiniatureFinish) EndTurnButtonAction();
        }

        private string NextTurn()
        {
            // int next = _players.FindIndex(f => f == _turnPlayer) + 1;

            // if (next >= _players.Count)
            //     return _players[0];

            return ""; //_players[next];
        }

        public void EndTurnButtonAction()
        {
            // when finish the preparation step
            if (turnStage == TurnStageEnum.Preparation)
            {
                SetIsReadyClient();
                turnStage = TurnStageEnum.GamePlay;
                Reset();

                return;
            }

            if (IsAllReadyToInitGame())
            {
                _turnPlayer = NextTurn();

                if (IsMyTurn())
                {
                    Reset();
                    OnStartTurnPlayer?.Invoke();
                }
            }
        }


        public void OnStartClient()
        {
            turnStage = TurnStageEnum.Preparation;

            _rules = new Dictionary<TurnStageEnum, Func<bool>>
            {
                { TurnStageEnum.Preparation, () => _amountCardUsed < GameManager.Instance.gameSettings.amountPlayCardOnPreparation },
                { TurnStageEnum.GamePlay, () => _amountCardUsed < GameManager.Instance.gameSettings.amountPlayCardOnGameplay }
            };
        }

        public void Load()
        {
            // add the players on turns
            if (GameManager.Instance.IsHost)
                foreach (string playerSessionId in GameManager.Instance.matchManager.Players)
                {
                    _players.Add(playerSessionId);
                }

            // choose a player to start turn
            int randomPlayerStartTurn = (int)MathF.Round(UnityEngine.Random.value);
            _turnPlayer = _players[randomPlayerStartTurn];
        }
    }
}
