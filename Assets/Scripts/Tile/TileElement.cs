using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class TileElement : MonoBehaviour
    {
        public Tile self { get; protected set; }

        public void SetTile(Tile tile) => self = tile;
    }
}
