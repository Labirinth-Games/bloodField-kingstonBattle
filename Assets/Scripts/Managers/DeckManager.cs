using Generators;
using Render;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class DeckManager : MonoBehaviour
    {
        [SerializeField] private List<CardSO> deck;

        [Header("References")]
        public DeckGenerate deckGenerate;

        private int _cardRemaing;

        public List<CardSO> Draw(int amount = 1)
        {
            var cards = new List<CardSO>();
            _cardRemaing -= amount;

            for(int i = 0; i< amount; i++)
            {
                var card = deck[Random.Range(0, deck.Count)];

                cards.Add(card);                
            }

            return cards;
        }

        public CardSO Draw()
        {
            _cardRemaing--;

             return deck[Random.Range(0, deck.Count)];
        }

        public bool CanDraw() => _cardRemaing > 0;

        public void Load()
        {
            _cardRemaing = GameManager.Instance.matchConfig.deckAmount;
            deck = deckGenerate.Deck();
            
        }

        private void OnValidate()
        {
            if(TryGetComponent(out DeckGenerate deckGenerate))
                this.deckGenerate = deckGenerate;
        }
    }
}
