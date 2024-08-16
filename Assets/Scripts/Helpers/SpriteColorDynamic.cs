using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Helpers
{
    public class SpriteColorDynamic : MonoBehaviour
    {
        public static Sprite ChangeColorBase(Sprite spriteOriginal, ColorConfigSO colors, bool isRemote = false)
        {
            if (!spriteOriginal.texture.isReadable)
            {
                Debug.LogError($"Cant access image {spriteOriginal.name}, check to read/write to continue");
                return spriteOriginal;
            }

            Texture2D texture = new Texture2D(spriteOriginal.texture.width, spriteOriginal.texture.height);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, spriteOriginal.texture.width, spriteOriginal.texture.height), Vector2.one / 2, spriteOriginal.pixelsPerUnit);

            sprite.texture.filterMode = FilterMode.Point;
            var color = isRemote ? colors.remoteMiniatureColor : colors.localMiniatureColor;

            for (int y = 0; y < spriteOriginal.texture.height; y++)
                for (int x = 0; x < spriteOriginal.texture.width; x++)
                {
                    Color pixelColor = spriteOriginal.texture.GetPixel(x, y);
                    Debug.Log(pixelColor.ToHexString());

                    if (pixelColor.ToHexString() == colors.baseColor.primary.ToHexString()) // primary color
                        sprite.texture.SetPixel(x, y, color.primary);
                    else if (pixelColor.ToHexString() == colors.baseColor.shadow.ToHexString())
                        sprite.texture.SetPixel(x, y, color.shadow);
                    else if (pixelColor.ToHexString() == colors.baseColor.light.ToHexString())
                        sprite.texture.SetPixel(x, y, color.light);
                    else
                        sprite.texture.SetPixel(x, y, pixelColor);
                }

            sprite.texture.Apply();

            return sprite;
        }
    }
}