using UnityEngine;
using Mirror;

namespace Tiles
{
    public class TileElement : NetworkBehaviour
    {
        public Tile self { get; protected set; }

        public void SetTile(Tile tile) => self = tile; 
    }
}
