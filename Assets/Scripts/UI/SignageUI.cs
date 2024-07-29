using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BloodField.Managers;
using BloodField.SO;
using BloodField.Types;
using Tiles;
using UnityEngine;

public class SignageUI : MonoBehaviour
{
    [SerializeField] private OverlayerConfigSO overlayerSprite;

    private List<GameObject> _instances = new List<GameObject>();

    public void Overlay(List<Tile> tiles, OverlayerType overlayerType, bool isOpacity = false) {
        MarkUI(tiles, overlayerSprite.GetSprite(overlayerType), isOpacity);
    }

    public void Overlay(List<(int y, int x)> positions, OverlayerType overlayerType, bool isOpacity = false)
    {
        List<Tile> tiles = positions.Select(f => new Tile(f.y, f.x)).ToList();
        MarkUI(tiles, overlayerSprite.GetSprite(overlayerType), isOpacity);
    }

    private void MarkUI(List<Tile> tiles, Sprite sprite, bool isOpacity = false)
    {
        tiles.ForEach(tile =>
        {
            var map = GameObject.Find("Map/OverlayActions");
            var instance = new GameObject();
            instance.AddComponent<SpriteRenderer>();

            instance.GetComponent<SpriteRenderer>().sprite = sprite;
            instance.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
            instance.transform.SetParent(map.transform);

            if (isOpacity)
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
