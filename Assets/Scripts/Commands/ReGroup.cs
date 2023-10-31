using Enums;
using Helpers;
using Managers;
using Miniatures;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Commands
{
    public class ReGroup : ActionCommand
    {
        private List<Miniature> _miniatures;

        #region Strategy
        private void MoveArmy((int y, int x) pos)
        {
            _miniatures = GameManager.Instance.miniatureManager.GetMiniatures().FindAll(f => f.stats.type == CardTypeEnum.Army);
            int miniatureIndex = 0;

            var tiles = ScanHelper.Scan(new Tile(pos.y, pos.x, TileTypeEnum.None), ScanDirectionTypeEnum.Ring, 1);
            tiles.Reverse();

            RegrupmentRecursive(tiles, miniatureIndex);
        }

        private void RegrupmentRecursive(List<Tile> tiles, int miniatureIndex)
        {
            if (miniatureIndex >= _miniatures.Count || tiles.Count == 0)
                return;

            foreach (Tile tile in tiles)
            {
                if (miniatureIndex >= _miniatures.Count)
                {
                    return;
                }

                _miniatures[miniatureIndex].self.MoveTo(tile.position);
                miniatureIndex++;
            }

            var randomTile = tiles[Random.Range(0, tiles.Count)];
            RegrupmentRecursive(ScanHelper.Scan(new Tile(randomTile.position.y, randomTile.position.x), ScanDirectionTypeEnum.Ring, 1), miniatureIndex);
        }
        #endregion

        public override void Action((int y, int x) pos)
        {
            MoveArmy(pos);
        }
    }
}
