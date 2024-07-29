using System;
using System.Collections.Generic;
using BloodField.Types;
using BloodField.Types;

namespace BloodField.Network.Entities
{
    [Serializable]
    public class MatchNetworkEntity : AuthorityNetworkEntity
    {
        public PhaseType matchState;
        public bool isFinishMatchLoad = false;
        public List<string> Players;
    }
}