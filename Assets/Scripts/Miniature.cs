using Managers;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Tiles;
using UnityEngine;

public class Miniature : TileElement
{
    public CardSO stats { get; private set; }

    public void Create((int y, int x) pos, Card card)
    {
        self = GameManager.Instance.mapManager.Register(new Tile(card.stats.type, gameObject), pos);
        self.SetPositionOnWorld();

        stats = card.stats;
    }
}
