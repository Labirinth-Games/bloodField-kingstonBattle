using BloodField.Types;
using BloodField.Managers;
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
            MatchConfigSO matchConfig = GameManager.Instance.MatchSettings;

            foreach (var cardSetting in matchConfig.DeckCardTypeAmount)
            {
                CardType CardType = cardSetting.Key;

                for (var i = 0; i < cardSetting.Value; i++)
                {
                    var cards = GameManager.Instance.cardManager.GetCardByType(CardType);

                    if (cards.Count > 0)
                    {
                        int indexRandom = Random.Range(0, cards.Count);
                        CardSO card = Instantiate(cards[indexRandom]);

                        if (CardType == CardType.Army && Random.Range(0f, 1f) <= GameManager.Instance.MatchSettings.probabilityOfHasArmyWithGroup) card.isGroup = true;

                        deck.Add(card);
                    }
                }
            }

            return deck;
        }
    }
}
