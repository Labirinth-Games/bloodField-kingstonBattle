using Generators;
using Mirror;
using Render;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class DeckManager : NetworkBehaviour
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

        public void Shuffle()
        {
            CardSO aux;
            List<CardSO> list = deck.ToList();
            Queue<CardSO> cards = new Queue<CardSO>();

            for(var i = 0; i < list.Count; i++)
            {
                int id1 = Random.Range(0, list.Count);
                int id2 = Random.Range(0, list.Count);

                aux = list[id1];
                list[id1] = list[id2];
                list[id2] = aux;
            }

            list.ForEach(f => cards.Enqueue(f));

            deck = cards;
        }

        #region Validatior
        public bool CanDraw() => deck.Count > 0 && _amountCardOnPlayerHand <= GameManager.Instance.gameSettings.maxCardOnPlayerHand;
        #endregion

        #region Gets/Sets
        public Queue<CardSO> GetDeck() => deck;
        public Queue<CardSO> SettDeck(Queue<CardSO> deck) => this.deck = deck;
        #endregion

        public void UseCardOnPlayerHand() => _amountCardOnPlayerHand--;

        public void Load()
        {
            deck = deckGenerate.Deck();
            Shuffle(); // shuffle cards
        }
    }
}
