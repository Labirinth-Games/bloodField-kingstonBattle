using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPreview : MonoBehaviour
{
    [SerializeField] private CardUI cardUI;

    public CardSO stats { get; private set; }

    public void Create(CardSO cardStats)
    {
        cardUI.Render(cardStats);

        stats = Instantiate(cardStats);
    }

    #region Unity Event
    private void OnValidate()
    {
        if (TryGetComponent(out CardUI cardUI))
            this.cardUI = cardUI;
    }
    #endregion
}
