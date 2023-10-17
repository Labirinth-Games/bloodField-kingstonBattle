using Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
public class CardSO : ScriptableObject
{
    public string title;
    [TextArea()]
    public string description;
    public CardTypeEnum type;
    public Sprite sprite;

    [Header("Stats")]
    public int ATK;
    public int DEF;
    public int MOV;
    public int D_ATK;

    [Header("Flags")]
    public bool isGroup;    
}

