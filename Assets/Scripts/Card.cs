using Managers;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private CardUI cardUI;

    public CardSO stats { get; private set; }
    public string _id { get; private set; } = System.Guid.NewGuid().ToString();

    public void Create(CardSO cardStats)
    {
        cardUI.Render(cardStats);

        stats = Instantiate(cardStats);
    }

    public void SetStats(CardSO stats) => this.stats = stats;

    private void OnValidate()
    {
        if (TryGetComponent(out CardUI cardUI))
            this.cardUI = cardUI;
    }
}
