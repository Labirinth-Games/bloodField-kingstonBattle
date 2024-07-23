using System;
using System.Collections.Generic;
using Enums;

namespace BloodField.Network.Entities
{
    [Serializable]
    public class TurnNetworkEntity : AuthorityNetworkEntity
    {
        public TurnStageEnum turnStage;
        public string currentTurnPlayerId;
    }
}