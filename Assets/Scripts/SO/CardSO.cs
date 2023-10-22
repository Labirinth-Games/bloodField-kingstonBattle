using System;
using Enums;
using Commands;
using UnityEngine;
using CustomAttributes;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
public class CardSO : ScriptableObject
{
    [Header("Settings")]
    public string title;
    [TextArea()]
    public string description;
    public Sprite sprite;
    [Space()]
    public CardTypeEnum type;
    [ConditionalItem(nameof(type), CardTypeEnum.Equipament)]
    public EquipamentTypeEnum equipamentType;
    public ScanDirectionTypeEnum direction;

    [Header("Stats")]
    public int ATK;
    public int DEF;
    public int MOV;
    public int D_ATK;

    [ConditionalItem(nameof(type), CardTypeEnum.Army)]
    public bool isGroup;

    [ConditionalItem(nameof(type), CardTypeEnum.Command)]
    public ActionCommand commandScript;
}

