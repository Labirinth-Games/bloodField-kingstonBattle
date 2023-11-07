using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public UnityEvent OnStartTurnPlayer;

        [SyncVar] private uint _turnPlayer;
        private SyncList<bool> _isAllReady = new SyncList<bool>();
        private SyncList<uint> _players = new SyncList<uint>();

        private int _amountCardUsed = 0;
        private bool _isAllMiniatureFinish = false;
        private Dictionary<TurnStageEnum, Func<bool>> _rules;

        #region Gets/Sets
        public bool IsMyTurn() => _turnPlayer == GameManager.Instance.player.netId;
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

        [Command(requiresAuthority = false)]
        public void SetIsReadyClient() => _isAllReady.Add(true);
        #endregion

        private void AutomaticEndTurn()
        {
            if (CanPlayCard() && _isAllMiniatureFinish) EndTurnServerRpc();
        }

        private uint NextTurn()
        {
            int next = _players.FindIndex(f => f == _turnPlayer) + 1;

            if (next >= _players.Count)
                return _players[0];

            return _players[next];
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
                EndTurnServerRpc();
        }

        [Command(requiresAuthority = false)]
        public void EndTurnServerRpc()
        {
            EndTurnClientRpc();
        }

        [ClientRpc]
        public void EndTurnClientRpc()
        {
            _turnPlayer = NextTurn();

            if (IsMyTurn())
            {
                Reset();
                OnStartTurnPlayer?.Invoke();
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

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
            if (isServer)
                foreach (Player player in GameManager.Instance.networkManager.playersInGame)
                {
                    _players.Add(player.netId);
                }

            // choose a player to start turn
            int randomPlayerStartTurn = (int)MathF.Round(UnityEngine.Random.value);
            _turnPlayer = _players[randomPlayerStartTurn];
        }
    }
}
