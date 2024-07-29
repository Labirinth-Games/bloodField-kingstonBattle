using System;
using BloodField.Types;
using BloodField.Domain.Commands;
using UnityEngine;
using CustomAttributes;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
public class CardSO : ScriptableObject
{
    [Space()]
    public string title;
    [TextArea()]
    public string description;

    [Space()]
    [Header("Skin")]
    // public Color primaryColor;
    // public Color secundaryColor;
    public Sprite sprite;

    [Space()]
    [Header("Setting")]
    public CardType type;


    // settings to equipaments

    [ConditionalItem(nameof(type), CardType.Equipament)]
    public EquipamentType equipamentType;

    [ConditionalItem(nameof(type), CardType.Equipament)]
    public WeightEquipamentType weightEquipamentType;

    [ConditionalItem(nameof(type), CardType.Equipament)]
    public bool isIndestructible = false;

    // settings to armary

    [ConditionalItem(nameof(type), CardType.Army)]
    public ArmyType armyType;

    // settings to terrain
    
    [ConditionalItem(nameof(type), CardType.Terrain)]
    public bool canApplyEffectToAllMap;
    [ConditionalItem(nameof(type), CardType.Terrain)]
    public int width;
    [ConditionalItem(nameof(type), CardType.Terrain)]
    public int height;
    [ConditionalItem(nameof(type), CardType.Terrain)]
    public int turnDuration;
    [ConditionalItem(nameof(type), CardType.Terrain)]
    public Sprite effectSprite;
    [ConditionalItem(nameof(type), CardType.Terrain)]
    public ParticleSystem effectVFX;


    [Space()]
    [Header("Stats")]
    [ConditionalItem(nameof(type), new object[] { CardType.Army, CardType.Equipament, CardType.King })]
    public ScanDirectionType direction;

    [ConditionalItem(nameof(type), new object[] { CardType.Army, CardType.Equipament, CardType.King })]
    public int ATK;
    [ConditionalItem(nameof(type), new object[] { CardType.Army, CardType.Equipament, CardType.King })]
    public int DEF;
    [ConditionalItem(nameof(type), new object[] { CardType.Army, CardType.Equipament, CardType.King })]
    public int MOV;
    [ConditionalItem(nameof(type), new object[] { CardType.Army, CardType.Equipament, CardType.King })]
    public int D_ATK;

    [ConditionalItem(nameof(type), CardType.Army)]
    public bool isGroup;

    [ConditionalItem(nameof(type), CardType.Command)]
    public ActionCommand commandScript;

    [ConditionalItem(nameof(type), CardType.Terrain)]
    public ActionCommand customTerrainScript;

    [Space()]
    [Header("Effects")]
    [Space()]
    [SerializedDictionary("Prop Name", "value")]
    public SerializedDictionary<StatsType, int> additionalStats = new SerializedDictionary<StatsType, int>() {
        {StatsType.ATK, 0},
        {StatsType.DEF, 0},
        {StatsType.MOV, 0},
        {StatsType.D_ATK, 0},
    };
    public List<ArmyType> targets;

    private int GetValue(StatsType statsType, int baseValue)
    {
        int value;
        if (additionalStats.TryGetValue(statsType, out value) && type == CardType.Army)
            return baseValue + value;

        return baseValue;
    }
    public int GetATK() => GetValue(StatsType.ATK, ATK);
    public int GetDEF() => GetValue(StatsType.DEF, DEF);
    public int GetMOV() => GetValue(StatsType.MOV, MOV);
    public int GetD_ATK() => GetValue(StatsType.D_ATK, D_ATK);
}
