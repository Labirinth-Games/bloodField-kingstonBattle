using BloodField.Enums;
using BloodField.Helpers;
using BloodField.Network.Entities;
using Generators;
using Helpers;
using Nakama;
using Nakama.TinyJson;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BloodField.Managers
{
    public class DeckManager : MonoBehaviour
    {
        [SerializeField] private Queue<CardSO> deck = new Queue<CardSO>();

        [Header("References")]
        public DeckGenerate deckGenerate;

        public async Task Draw(int amount = 1)
        {
            if (!GameManager.Instance.IsHost)
            {
                await NetworkHelper.Send<DeckNetworkEntity>(
                    OpCodeEnum.DECK_DRAW_CARD,
                    new DeckNetworkEntity() { AmountCardsDraw = amount }
                );

                return;
            }

            var cards = GetCardsOnDeck(amount);

            // add cards diretly on hand to player
            GameManager.Instance.player.AddCardHand(cards);
        }

        #region Validatior
        public bool CanDraw(int amountCardOnPlayerHand) => deck.Count > 0 && amountCardOnPlayerHand <= GameManager.Instance.gameSettings.maxCardOnPlayerHand;
        #endregion

        #region Gets/Sets
        public void SetDeck(List<CardSO> cards)
        {
            deck.Clear();

            foreach (var item in cards)
            {
                deck.Enqueue(item);
            }
        }

        private List<CardSO> GetCardsOnDeck(int amount)
        {
            List<CardSO> cards = new List<CardSO>();

            for (int i = 0; i < amount; i++)
            {
                var card = deck.Dequeue();
                cards.Add(card);
            }

            return cards;
        }
        #endregion

        #region Network Events
        private void OnReceiveMatchState(IMatchState matchState)
        {
            NetworkHelper.Listen<DeckNetworkEntity>(matchState, OpCodeEnum.DECK_DRAW_CARD, async (content, isHost, isOwner) =>
            {
                if (isOwner) return;

                var cards = GetCardsOnDeck(content.AmountCardsDraw);
                var cardsPaths = cards.Select(s => $"Cards/{s.type}/{s.name}").ToArray();

                await NetworkHelper.Send<DeckNetworkEntity>(OpCodeEnum.DECK_RECEIVE_CARDS, new DeckNetworkEntity() { Cards = cardsPaths });
            });

            NetworkHelper.Listen<DeckNetworkEntity>(matchState, OpCodeEnum.DECK_RECEIVE_CARDS, (content, isHost, isOwner) =>
            {
                if (isOwner) return;
                
                var cards = content.GetCards();

                // add cards diretly on hand to player
                GameManager.Instance.player.AddCardHand(cards);
            });
        }

        #endregion

        public void Load()
        {
            var cards = deckGenerate.Deck();

            // if debug mode active get cards defined on list
            if (GameManager.Instance.isDebug)
                GetComponent<DeckDebug>().DeckTest();
            else
                SetDeck(cards);

            var cardsShaffled = DeckHelper.Shuffle(deck);
            SetDeck(cardsShaffled);

            // subscribers
            Subscribers();
        }

        public void Subscribers()
        {
            if (GameManager.Instance.networkManager.Socket is not null)
                GameManager.Instance.networkManager.Socket.ReceivedMatchState += OnReceiveMatchState;
        }
    }
}
