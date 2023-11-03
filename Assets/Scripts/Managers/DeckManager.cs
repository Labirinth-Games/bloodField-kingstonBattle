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
        // [SerializeField] private Queue<CardSO> deck;
        public readonly SyncList<CardSO> deck = new SyncList<CardSO>();

        [Header("References")]
        public DeckGenerate deckGenerate;

        private int _amountCardOnPlayerHand;

        public void Draw(int amount = 1)
        {
            DrawCardServerRpc(amount);
        }

        [Command(requiresAuthority = false)]
        public void DrawCardServerRpc(int amount, NetworkConnectionToClient sender = null)
        {
            List<CardSO> _cardsAux = new List<CardSO>();

            for (int i = 0; i < amount; i++)
            {
                var card = deck[0];
                deck.RemoveAt(0);

                _cardsAux.Add(card);
            }

            DrawCardsClient(new CardDrawSerializerNetwork(_cardsAux), amount);
        }

        [ClientRpc]
        public void DrawCardsClient(CardDrawSerializerNetwork cardDraw, int amount)
        {
            _amountCardOnPlayerHand += amount;

            Debug.Log($"foram pegas {amount} cartas e sobrou {deck.Count}");
            
            GameManager.Instance.cardManager.Create(cardDraw.cards);
            GameManager.Instance.player.SetCardOnHand(cardDraw.cards);
        }

        public void Shuffle()
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

            SetDeck(list);
        }

        #region Validatior
        public bool CanDraw() => deck.Count > 0 && _amountCardOnPlayerHand <= GameManager.Instance.gameSettings.maxCardOnPlayerHand;
        #endregion

        #region Gets/Sets
        public SyncList<CardSO> GetDeck() => deck;
        public void SetDeck(List<CardSO> deck)
        {
            this.deck.Clear();

            foreach (var item in deck)
            {
                this.deck.Add(item);
            }
        }
        #endregion

        public void UseCardOnPlayerHand() => _amountCardOnPlayerHand--;

        public void Load()
        {
            SetDeck(deckGenerate.Deck());
            Shuffle(); // shuffle cards
        }
    }
}
