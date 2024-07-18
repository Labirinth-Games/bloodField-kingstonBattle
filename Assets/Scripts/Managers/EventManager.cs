using System;
using System.Collections.Generic;
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
    }
}