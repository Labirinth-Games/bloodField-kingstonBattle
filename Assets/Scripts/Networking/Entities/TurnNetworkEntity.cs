using System;
using System.Collections.Generic;
using BloodField.Types;

namespace BloodField.Network.Entities
{
    [Serializable]
    public class TurnNetworkEntity : AuthorityNetworkEntity
    {
        public TurnStageType turnStage;
        public string currentTurnPlayerId;
    }
}