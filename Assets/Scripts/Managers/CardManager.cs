using Cards;
using Render;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Managers
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] private List<CardSO> cards;
        public GameObject cardPreviewPrefab;
        public GameObject cardPrefab;

        private Dictionary<string, GameObject> _instances = new Dictionary<string, GameObject>();
        private GameObject _instancePreview;

        public List<CardSO> GetCardByType(CardTypeEnum type) => cards.FindAll(f => f.type == type);

        public void Create(List<CardSO> cards)
        {
            foreach(CardSO cardData in cards)
            {
                var card = CardRender.Render(cardData);
                _instances.Add(card._id, card.gameObject);
            }

            GameManager.Instance.mouseHelper.OnCardHoverEnter.AddListener((Card card) => card?.GetComponent<CardUI>()?.HoverEnter());
            GameManager.Instance.mouseHelper.OnCardHoverExit.AddListener((Card card) => card?.GetComponent<CardUI>()?.HoverExit());
            GameManager.Instance.mouseHelper.OnCardSelected.AddListener(RemoveCardUsed);
        }

        public void Preview(Card card)
        {
           ClosePreview();

            _instancePreview = CardRender.Render(card.stats, true).gameObject;
        }

        public void ClosePreview()
        {
            if (_instancePreview != null)
                Destroy(_instancePreview);
        }

        private void RemoveCardUsed(Card card)
        {
            GameObject cardSelected;

            if (_instances.TryGetValue(card._id, out cardSelected))
                Destroy(cardSelected);
        }
    }
}
