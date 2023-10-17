using Managers;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Render
{
    public class MapRender : MonoBehaviour
    {
        public static List<GameObject> FloorRender(List<Tile>[,] grid, List<Sprite> floorPrefabs, List<Sprite> baseSpawn, int spawnAreaScale)
        {
            List<GameObject> instances = new List<GameObject>();

            if (floorPrefabs.Count == 0)
            {
                Debug.LogWarning("You need to add the the floor prefabs");
                return instances;
            }

            for (var y = 0; y < grid.GetLength(0); y++)
                for (var x = 0; x < grid.GetLength(1); x++)
                {
                    var map = GameObject.Find("Map");
                    var instance = new GameObject();

                    instance.AddComponent<SpriteRenderer>();
                    instance.transform.position = new Vector3(x, y);
                    instance.transform.SetParent(map.transform);
                    instance.GetComponent<SpriteRenderer>().sprite = floorPrefabs[Random.Range(0, floorPrefabs.Count)];

                    if ((x >= 0 && x < grid.GetLength(1) && y >= 0 && y < spawnAreaScale) || ( x >= 0 && x < grid.GetLength(1) && y >= grid.GetLength(0) - spawnAreaScale && y < grid.GetLength(0)))
                        instance.GetComponent<SpriteRenderer>().sprite = baseSpawn[Random.Range(0, baseSpawn.Count)];
                    else if ((x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0))
                        instance.GetComponent<SpriteRenderer>().color = new Color(.8f, .8f, .8f, .95f);


                    instances.Add(instance);
                }

            return instances;
        }

        public static List<GameObject> KingRender(List<(int y, int x)> positions, GameObject prefab)
        {
            List<GameObject> instances = new List<GameObject>();

            foreach (var position in positions)
            {
                var instance = Instantiate(prefab);
                instance.GetComponent<King>().Create(position);

                instances.Add(instance);
            }

            return instances;
        }

        public static GameObject MiniatureRender(Card card)
        {
            (int y, int x) pos = (0, 0);

            var instance = new GameObject();
            instance.AddComponent<SpriteRenderer>().sprite = card.stats.sprite;
            instance.AddComponent<Miniature>().Create(pos, card);
            instance.GetComponent<SpriteRenderer>().sortingLayerName = "Miniature";

            instance.transform.SetParent(GameObject.Find("Miniatures").transform);

            return instance;
        }
    }
}
