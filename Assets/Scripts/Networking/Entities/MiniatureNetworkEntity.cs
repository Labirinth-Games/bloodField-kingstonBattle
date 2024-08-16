using System;
using System.Collections.Generic;
using BloodField.Types;
using UnityEngine;

namespace BloodField.Network.Entities
{
    [Serializable]
    public class MiniatureNetworkEntity : AuthorityNetworkEntity
    {
        public string id;
        public int y;
        public int x;
        public string cardPath;

        public int damage;

        public CardSO GetCard() => Resources.Load<CardSO>(cardPath.Replace("(Clone)", string.Empty));
        public (int, int) GetPosition() => (y, x);
    }
}