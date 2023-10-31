using Managers;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private CardUI cardUI;

    public CardSO stats { get; private set; }
    public string _id { get; private set; } = System.Guid.NewGuid().ToString();

    private bool _canPlayCard = true;

    public void Create(CardSO cardStats)
    {
        cardUI.Render(cardStats);
        stats = Instantiate(cardStats);

        GameManager.Instance.turnManager.OnDontUseCard.AddListener(() => _canPlayCard = false);
        GameManager.Instance.turnManager.OnUseCard.AddListener(() => _canPlayCard = true);
    }

    public void PlayingCard() {
        GameManager.Instance.miniatureManager.Build(stats);
    }

    #region Mouse Events
    public void OnPointerEnter(PointerEventData eventData)
    {
        cardUI.HoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardUI.HoverExit();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_canPlayCard) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameManager.Instance.turnManager.SetCardUsed();
            GameManager.Instance.cardManager.ClosePreview();
            cardUI.Click();
            
            PlayingCard();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
            GameManager.Instance.cardManager.Preview(this);
    }
    #endregion

    #region Unity Event
    private void OnValidate()
    {
        if (TryGetComponent(out CardUI cardUI))
            this.cardUI = cardUI;
    }
    #endregion
}
