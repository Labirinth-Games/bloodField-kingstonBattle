using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Helpers
{
    public struct SpriteColors
    {
        public Color primary;
        public Color secundary;
    }

    public class SpriteColorDynamic : MonoBehaviour
    {
        public static Sprite ChangeColorBase(Sprite spriteOriginal, SpriteColors colors)
        {
            if (!spriteOriginal.texture.isReadable)
            {
                Debug.LogError($"Cant access image {spriteOriginal.name}, check to read/write to continue");
                return spriteOriginal;
            }

            Texture2D texture = new Texture2D(spriteOriginal.texture.width, spriteOriginal.texture.height);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, spriteOriginal.texture.width, spriteOriginal.texture.height), Vector2.one / 2, spriteOriginal.pixelsPerUnit);

            sprite.texture.filterMode = FilterMode.Point;

            for (int y = 0; y < spriteOriginal.texture.height; y++)
                for (int x = 0; x < spriteOriginal.texture.width; x++)
                {
                    Color pixelColor = spriteOriginal.texture.GetPixel(x, y);
                    Debug.Log(pixelColor.ToHexString());

                    if (pixelColor.ToHexString() == "7E8AA7FF")
                        sprite.texture.SetPixel(x, y, colors.primary);
                    else if (pixelColor.ToHexString() == "566794FF")
                        sprite.texture.SetPixel(x, y, colors.secundary);
                    else
                        sprite.texture.SetPixel(x, y, pixelColor);
                }

            sprite.texture.Apply();

            return sprite;
        }
    }
}