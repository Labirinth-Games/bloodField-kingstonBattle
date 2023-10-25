using System;
using Enums;
using Commands;
using UnityEngine;
using CustomAttributes;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;

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

    [ConditionalItem(nameof(type), CardTypeEnum.Army)]
    public ArmyTypeEnum armyType;

    // settings to terrain
    [ConditionalItem(nameof(type), CardTypeEnum.Terrain)]
    public bool canApplyEffectToAllMap;
    [ConditionalItem(nameof(type), CardTypeEnum.Terrain)]
    public int width;
    [ConditionalItem(nameof(type), CardTypeEnum.Terrain)]
    public int height;
    [ConditionalItem(nameof(type), CardTypeEnum.Terrain)]
    public int turnDuration;
    [ConditionalItem(nameof(type), CardTypeEnum.Terrain)]
    public Sprite effectSprite;
    [ConditionalItem(nameof(type), CardTypeEnum.Terrain)]
    public ParticleSystem effectVFX;

    [ConditionalItem(nameof(type), new object[] { CardTypeEnum.Army, CardTypeEnum.Equipament, CardTypeEnum.King })]
    public ScanDirectionTypeEnum direction;

    [ConditionalItem(nameof(type), new object[] { CardTypeEnum.Army, CardTypeEnum.Equipament, CardTypeEnum.King })]
    public int ATK;
    [ConditionalItem(nameof(type), new object[] { CardTypeEnum.Army, CardTypeEnum.Equipament, CardTypeEnum.King })]
    public int DEF;
    [ConditionalItem(nameof(type), new object[] { CardTypeEnum.Army, CardTypeEnum.Equipament, CardTypeEnum.King })]
    public int MOV;
    [ConditionalItem(nameof(type), new object[] { CardTypeEnum.Army, CardTypeEnum.Equipament, CardTypeEnum.King })]
    public int D_ATK;

    [ConditionalItem(nameof(type), CardTypeEnum.Army)]
    public bool isGroup;

    [ConditionalItem(nameof(type), CardTypeEnum.Command)]
    public ActionCommand commandScript;

    [Space()]
    [Header("Effects")]
    [Space()]
    [SerializedDictionary("Prop Name", "value")]
    public SerializedDictionary<StatsTypeEnum, int> additionalStats = new SerializedDictionary<StatsTypeEnum, int>() {
        {StatsTypeEnum.ATK, 0},
        {StatsTypeEnum.DEF, 0},
        {StatsTypeEnum.MOV, 0},
        {StatsTypeEnum.D_ATK, 0},
    };
    public List<ArmyTypeEnum> targets;

    // public CardSO() {
    //     if (type != CardTypeEnum.Army || type != CardTypeEnum.King) return;

    //     additionalStats = new SerializedDictionary<StatsTypeEnum, int>();

    //     var enumValues = Enum.GetValues(typeof(StatsTypeEnum));
    //     for (var i = 0; i < enumValues.GetLength(0); i++)
    //     {
    //         additionalStats.Add((StatsTypeEnum)enumValues.GetValue(i), 0);
    //     }
    // }

    private int GetValue(StatsTypeEnum statsType, int baseValue)
    {
        int value;
        if (additionalStats.TryGetValue(statsType, out value) && type == CardTypeEnum.Army)
            return baseValue + value;

        return baseValue;
    }
    public int GetATK() => GetValue(StatsTypeEnum.ATK, ATK);
    public int GetDEF() => GetValue(StatsTypeEnum.DEF, DEF);
    public int GetMOV() => GetValue(StatsTypeEnum.MOV, MOV);
    public int GetD_ATK() => GetValue(StatsTypeEnum.D_ATK, D_ATK);
}
