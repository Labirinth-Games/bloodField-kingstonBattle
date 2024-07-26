using System;
using System.Collections.Generic;
using BloodField.Enums;
using BloodField.Helpers;
using BloodField.Network.Entities;
using Nakama;
using UnityEngine;

namespace BloodField.Managers
{
    public class EventManager : MonoBehaviour
    {
        #region Network Match Events
        public event Action<IMatchmakerMatched> OnReceivedMatchmakerMatched;
        public void ReceivedMatchmakerMatchedEvent(IMatchmakerMatched value)
        {
            if (OnReceivedMatchmakerMatched != null) OnReceivedMatchmakerMatched(value);
        }

        public event Action<IMatchPresenceEvent> OnReceivedMatchPresence;
        public void ReceivedMatchPresenceEvent(IMatchPresenceEvent value)
        {
            if (OnReceivedMatchPresence != null) OnReceivedMatchPresence(value);
        }

        public event Action<IStatusPresenceEvent> OnReceivedStatusPresence;
        public void ReceivedStatusPresenceEvent(IStatusPresenceEvent value)
        {
            if (OnReceivedStatusPresence != null) OnReceivedStatusPresence(value);
        }

        public event Action<IMatchState> OnReceivedMatchState;
        public void ReceivedMatchStateEvent(IMatchState value)
        {
            if (OnReceivedMatchState != null) OnReceivedMatchState(value);
        }
        #endregion

        #region HUD
        public event Action<string> OnDisplayMessageFindMatchHUD;
        public void DisplayMessageFindMatchHUDEvent(string value)
        {
            if (OnDisplayMessageFindMatchHUD != null) OnDisplayMessageFindMatchHUD(value);
        }
        public event Action<List<string>> OnDisplayPlayersOnLobbyHUD;
        public void DisplayPlayersOnLobbyHUDEvent(List<string> value)
        {
            if (OnDisplayPlayersOnLobbyHUD != null) OnDisplayPlayersOnLobbyHUD(value);
        }
        #endregion

        #region Turn
        public event Action OnStartMyTurn;
        public void StartMyTurnEvent()
        {
            if (OnStartMyTurn != null) OnStartMyTurn();
        }

        public event Action OnStartMainPhase;
        public async void StartMainPhase(bool eventRemote = false)
        {
            if (OnStartMainPhase != null) OnStartMainPhase();

            if (eventRemote) await NetworkHelper.Send<MatchNetworkEntity>(
                OpCodeEnum.MATCH_STATE,
                new MatchNetworkEntity() { matchState = PhaseEnum.Main }
            );
        }

        public event Action OnStartPreparationPhase;
        public void StartPreparationPhaseEvent()
        {
            if (OnStartPreparationPhase != null) OnStartPreparationPhase();
        }

        public event Action OnFinishedPreparationPhase;
        public void FinishedPreparationPhaseEvent()
        {
            if (OnFinishedPreparationPhase != null) OnFinishedPreparationPhase();
        }
        #endregion

        #region Card

        /// <summary>
        /// When player click on card and generate a miniature to add on board
        /// </summary>
        public event Action OnCardUsed;
        public async void CardUsedEvent(bool eventRemote = false)
        {
            if (OnCardUsed != null) OnCardUsed();

            if (eventRemote) await NetworkHelper.Send<TurnNetworkEntity>(
                OpCodeEnum.TURN_CARD_USED,
                new TurnNetworkEntity() {}
            );
        }
        #endregion

        #region Game
        public event Action OnGameLose;
        public void GameLoseEvent()
        {
            if (OnGameLose != null) OnGameLose();
        }

        public event Action OnGameWin;
        public void GameWinEvent()
        {
            if (OnGameWin != null) OnGameWin();
        }
        #endregion
    }
}