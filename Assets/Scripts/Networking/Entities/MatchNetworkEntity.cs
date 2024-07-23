using System;
using System.Collections.Generic;
using BloodField.Enums;
using Enums;

namespace BloodField.Network.Entities
{
    [Serializable]
    public class MatchNetworkEntity : AuthorityNetworkEntity
    {
        public PhaseEnum matchState;
        public bool isFinishMatchLoad = false;
        public List<string> Players;
    }
}