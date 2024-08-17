using System;
using System.Collections.Generic;
using BloodField.Types;
using BloodField.Helpers;
using BloodField.Network.Entities;
using Nakama;
using UnityEngine;
using Tiles;

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
                OpCodeType.MATCH_STATE,
                new MatchNetworkEntity() { matchState = PhaseType.Main }
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
                OpCodeType.TURN_CARD_USED,
                new TurnNetworkEntity() { }
            );
        }
        #endregion

        #region Miniature
        public event Action<string, CardSO, Tile> OnMiniatureCreated;
        public void MiniatureCreatedEvent(string id, CardSO card, Tile tile)
        {
            if (OnMiniatureCreated != null)  OnMiniatureCreated(id, card, tile);
        }

        public event Action<string, (int y, int x), CardSO, Tile> OnTerrainCreated;
        public void TerrainCreatedEvent(string id, (int y, int x) pos, CardSO card, Tile tile)
        {
            if (OnTerrainCreated != null)  OnTerrainCreated(id, pos, card, tile);
        }
        #endregion

        #region Game
        public event Action<bool> OnEndGame;
        public void EndGameEvent(bool isWin = false)
        {
            if (OnEndGame != null)
            {
                GameManager.Instance.matchManager.SetIsFinishGame(true);
                OnEndGame(isWin);
            }
        }
        #endregion
    }
}