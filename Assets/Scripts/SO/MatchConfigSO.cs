using AYellowpaper.SerializedCollections;
using Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "MatchConfig", menuName = "ScriptableObjects/Match Config", order = 2)]
public class MatchConfigSO : ScriptableObject
{
    [Header("Settings Game")]
    public string title;
    public int initialAmountInHand = 5; // initial number of cards the player has in hand
    public int amountPlayCardCanActive = 1; // amount that the player can play in your turn

    [Space()]
    [Header("Settings Deck")]
    [SerializedDictionary("Card Type", "Amount")]
    public SerializedDictionary<CardTypeEnum, int> DeckCardTypeAmount;

    public int deckAmount = 0;

    private void OnValidate()
    {
        deckAmount = 0;

        foreach (var kvp in DeckCardTypeAmount)
        {
            deckAmount += kvp.Value;
        }
    }
}

