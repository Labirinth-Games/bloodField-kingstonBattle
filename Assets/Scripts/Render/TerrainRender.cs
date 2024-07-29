using BloodField.Types;
using BloodField.Managers;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Render
{
    public class TerrainRender : MonoBehaviour
    {
        public static List<GameObject> Render(List<(int y, int x)> positions, GameObject gameObject, CardSO card, bool isTerrainPermanent = false)
        {
            var instances = new List<GameObject>();

            foreach (var position in positions)
            {
                var instance = new GameObject();
                var spriteRender = instance.AddComponent<SpriteRenderer>();
                spriteRender.sprite = card.effectSprite;
                spriteRender.sortingOrder = card.sortIndexLayerSprite;
                spriteRender.sortingLayerName = card.sortLayerSprite;

                // when terrain permanent all middle map change terrain but continue gusty
                if (isTerrainPermanent && ((position.x % 2 == 0 && position.y % 2 != 0) || (position.x % 2 != 0 && position.y % 2 == 0)))
                    spriteRender.color = new Color(.8f, .8f, .8f, .95f);
                else
                    spriteRender.color = new Color(1, 1, 1, card.opacitySprite);

                var newTile = GameManager.Instance.mapManager.Register(new Tile(TileType.Terrain, gameObject), position, true);
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
