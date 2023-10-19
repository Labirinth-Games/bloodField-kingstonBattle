using Generators;
using Render;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class DeckManager : MonoBehaviour
    {
        [SerializeField] private Queue<CardSO> deck;

        [Header("References")]
        public DeckGenerate deckGenerate;

        private int _amountCardOnPlayerHand;

        public void Draw(int amount = 1)
        {
            var cards = new List<CardSO>();
            _amountCardOnPlayerHand += amount;

            for(int i = 0; i< amount; i++)
            {
                var card = deck.Dequeue();

                cards.Add(card);                
            }

            GameManager.Instance.cardManager.Create(cards);
            GameManager.Instance.player.SetCardOnHand(cards);
        }

        public bool CanDraw() => deck.Count > 0 && _amountCardOnPlayerHand <= GameManager.Instance.matchConfig.maxCardOnPlayerHand;

        public void UseCardOnPlayerHand() => _amountCardOnPlayerHand--;

        public void Load()
        {
            deck = deckGenerate.Deck();            
        }

        private void OnValidate()
        {
            if(TryGetComponent(out DeckGenerate deckGenerate))
                this.deckGenerate = deckGenerate;
        }
    }
}
