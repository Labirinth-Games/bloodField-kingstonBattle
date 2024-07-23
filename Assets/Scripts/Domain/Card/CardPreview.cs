using BloodField.Managers;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPreview : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private CardUI cardUI;

    public CardSO stats { get; private set; }

    private GameObject parent;

    public void Create(CardSO cardStats, GameObject parent)
    {
        cardUI.Render(cardStats);
        stats = Instantiate(cardStats);

        this.parent = parent;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!GameManager.Instance.matchManager.CanPlayCard()) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameManager.Instance.cardManager.ClosePreview();
            parent.GetComponent<Card>().PlayingCard();

            cardUI.Click();
            Destroy(parent, .2f);
        }
    }

    #region Unity Event
    private void OnValidate()
    {
        if (TryGetComponent(out CardUI cardUI))
            this.cardUI = cardUI;
    }
    #endregion
}
