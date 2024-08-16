using UnityEngine;

[CreateAssetMenu(fileName = "ColorConfig", menuName = "ScriptableObjects/Color Config", order = 2)]
public class ColorConfigSO : ScriptableObject
{
    [Header("Settings color")]
    public SpriteColors baseColor;
    public SpriteColors localMiniatureColor;
    public SpriteColors remoteMiniatureColor;
}

[System.Serializable]
public struct SpriteColors
{
    public Color primary;
    public Color shadow;
    public Color light;
}