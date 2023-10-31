using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private List<CardSO> hand = new List<CardSO>();

    public void SetCardOnHand(List<CardSO> cards) => hand.AddRange(cards);
}
