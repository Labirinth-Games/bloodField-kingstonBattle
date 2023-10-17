using Cards;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generators
{
    public class DeckGenerate : MonoBehaviour
    {
        public List<CardSO> Deck()
        {
            List<CardSO> deck = new List<CardSO>();
            MatchConfigSO matchConfig = GameManager.Instance.matchConfig;

            foreach (var card in matchConfig.DeckCardTypeAmount)
            {
                CardTypeEnum CardType = card.Key;

                for(var i = 0; i < card.Value; i++)
                {
                    var cards = GameManager.Instance.cardManager.GetCardByType(CardType);

                    if(cards.Count > 0)
                    {
                        int indexRandom = Random.Range(0, cards.Count);

                        deck.Add(cards[indexRandom]);
                    }
                }
            }

            return deck;
        }
    }
}
