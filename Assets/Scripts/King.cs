using Managers;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

public class King : TileElement
{
    [SerializeField] private Sprite kingSprite;

    public void Create((int y, int x) pos)
    {
        self = GameManager.Instance.mapManager.Register(new Tile(TileTypeEnum.King, gameObject), pos);
        self.SetPositionOnWorld();
    }
}
