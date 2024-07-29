using AYellowpaper.SerializedCollections;
using BloodField.Types;
using UnityEngine;

namespace BloodField.SO
{
    [CreateAssetMenu(fileName = "OverlayerConfigSO", menuName = "ScriptableObjects/Overlayer Config", order = 0)]
    public class OverlayerConfigSO : ScriptableObject
    {
        [SerializedDictionary("Type", "Sprite Reference")]
        public SerializedDictionary<OverlayerType, Sprite> overlayerSprites;

        public Sprite GetSprite(OverlayerType type)
        {
            Sprite sprite;

            overlayerSprites.TryGetValue(type, out sprite);

            return sprite;
        }
    };
}