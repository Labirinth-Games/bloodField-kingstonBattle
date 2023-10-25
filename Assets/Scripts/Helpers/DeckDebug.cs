using AYellowpaper.SerializedCollections;
using Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    public class DeckDebug : MonoBehaviour
    {
        [Header("Debug")]

        [SerializedDictionary("Card Type", "Amount")]
        public SerializedDictionary<CardSO, int> cards = new SerializedDictionary<CardSO, int>();

        public void DeckTest()
        {
            Queue<CardSO> deck = new Queue<CardSO>();

            foreach (var card in cards)
            {
                for (var i = 0; i < card.Value; i++)
                    deck.Enqueue(card.Key);
            }

            GetComponent<DeckManager>().SettDeck(deck);
        }
    }
}
