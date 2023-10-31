using Enums;
using Managers;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Render
{
    public class TerrainRender : MonoBehaviour
    {
        public static List<GameObject> Render(List<(int y, int x)> positions, GameObject gameObject, Sprite sprite)
        {
            var instances = new List<GameObject>();

            foreach (var position in positions)
            {
                var instance = new GameObject();
                var spriteRender = instance.AddComponent<SpriteRenderer>();
                spriteRender.sprite = sprite;
                spriteRender.sortingOrder = 1;
                spriteRender.color = new Color(1, 1, 1, .5f);

                var newTile = GameManager.Instance.mapManager.Register(new Tile(TileTypeEnum.Terrain, gameObject), position, true);
                instance.AddComponent<TileElement>().SetTile(newTile);

                instance.transform.position = newTile.GetPositionOnWorld();

                instances.Add(instance);
            }

            return instances;
        }

        public static ParticleSystem VfxRender(ParticleSystem particle)
        {
            var instance = Instantiate(particle);
            instance.GetComponent<ParticleSystem>().Play();

            var camPosition = Camera.main.transform.position;
            instance.transform.position = new Vector3(camPosition.x, camPosition.y * 2.5f, 0);

            return instance;
        }
    }
}
