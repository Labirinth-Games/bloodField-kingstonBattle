using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using UnityEngine.Events;

public class SignageUI : MonoBehaviour
{
    [SerializeField] private Sprite moveOverlayerSprite;
    [SerializeField] private Sprite reachOverlayerSprite;

    private List<GameObject> _instances = new List<GameObject>();

    public void OverlayAttack(List<Tile> tiles) => MarkUI(tiles, reachOverlayerSprite, true);
    public void OverlayMove(List<Tile> tiles) => MarkUI(tiles, moveOverlayerSprite);

    private void MarkUI(List<Tile> tiles, Sprite sprite, bool isOpacity = false)
    {
        tiles.ForEach(tile =>
        {
            var instance = new GameObject();
            instance.AddComponent<SpriteRenderer>();

            instance.GetComponent<SpriteRenderer>().sprite = sprite;
            instance.GetComponent<SpriteRenderer>().sortingLayerName = "UI";

            if(isOpacity)
            {
                instance.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .5f);
                instance.GetComponent<SpriteRenderer>().sortingOrder = -1;
            }

            instance.transform.position = tile.GetPositionOnWorld();

            _instances.Add(instance);
        });
    }

    public void Clear()
    {
        foreach (var instance in _instances)
        {
            Destroy(instance);
        }

        _instances.Clear();
    }
}
