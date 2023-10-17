using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private List<CardSO> hand;

    public void SetCardHand(List<CardSO> cards) => hand = cards;
}
