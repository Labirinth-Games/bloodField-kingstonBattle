using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BloodField.Helpers {
    public class DeckHelper {
        public static List<CardSO> Shuffle(Queue<CardSO> deck)
        {
            CardSO aux;
            List<CardSO> list = deck.ToList();

            for (var i = 0; i < list.Count; i++)
            {
                int id1 = Random.Range(0, list.Count);
                int id2 = Random.Range(0, list.Count);

                aux = list[id1];
                list[id1] = list[id2];
                list[id2] = aux;
            }

            return list;
        }
    }
}