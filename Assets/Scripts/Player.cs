using BloodField.Managers;
using System.Collections.Generic;
using UnityEngine;
using Render;
using BloodField.Helpers;
using BloodField.Network.Entities;
using BloodField.Types;
using Nakama;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject kingPrefab;

    [SerializeField] private List<CardSO> hand;

    public void RemoveCardHand(CardSO card) => hand.Remove(card);
    public void AddCardHand(List<CardSO> cards)
    {
        hand.AddRange(cards);
        GameManager.Instance.cardManager.Create(cards);
    }

    #region Turn
    public async void MyTurn()
    {
        if (GameManager.Instance.deckManager.CanDraw(hand.Count)) await GameManager.Instance.deckManager.Draw();
    }
    #endregion

    public void Load()
    {
        hand.Clear();
        MiniatureRender.KingRender(kingPrefab);

        // subscribers
        GameManager.Instance.eventManager.OnStartMyTurn += MyTurn;
    }
}
