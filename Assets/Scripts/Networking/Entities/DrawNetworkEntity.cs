using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BloodField.Network.Entities
{
    [Serializable]
    public class DeckNetworkEntity : AuthorityNetworkEntity
    {
        public string[] Cards;
        public int AmountCardsDraw;

        public List<CardSO> GetCards() => Cards.Select(s => Resources.Load<CardSO>(s)).ToList();
    }
}