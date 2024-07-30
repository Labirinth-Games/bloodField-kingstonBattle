using AYellowpaper.SerializedCollections;
using BloodField.Managers;
using BloodField.Types;
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
            List<CardSO> deck = new List<CardSO>();

            foreach (var card in cards)
            {
                for (var i = 0; i < card.Value; i++) {
                    var item = Instantiate(card.Key);

                    if (card.Key.type == CardType.Army && Random.Range(0f, 1f) <= GameManager.Instance.MatchSettings.probabilityOfHasArmyWithGroup) item.isGroup = true;
                    deck.Add(item);
                }
            }

            GetComponent<DeckManager>().SetDeck(deck);
        }
    }
}
