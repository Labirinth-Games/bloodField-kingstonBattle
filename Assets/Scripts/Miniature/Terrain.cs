using Helpers;
using Managers;
using Render;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Miniatures
{
    public class Terrain : Miniature
    {
        private (int y, int x) _lastPosition;
        private List<Tile> _tarrainArea;
        private bool _canApplyEffectToAllMap = true;

        #region Gets/Sets
        public override bool CanAddOnBoard((int y, int x) position) => GameManager.Instance.mapManager.CanSpawnUntilMiddleMiniatures(position);
        #endregion

        public override void AddOnBoard((int y, int x) pos)
        {
            //stats.commandScript.Action(pos); // call the command specific
            Destroy(gameObject, .2f);
        }

        public override void Create((int y, int x) pos, CardSO card)
        {
            base.Create(pos);

            self = GameManager.Instance.mapManager.Register(new Tile(card.type, gameObject), pos);
            self.SetPositionOnWorld();

            stats = Instantiate(card);
            self = new Tile(0, 0, Enums.TileTypeEnum.None);
            _lastPosition = self.position;
            _canApplyEffectToAllMap = stats.canApplyEffectToAllMap;
        }

        private void Update()
        {
            if(_canApplyEffectToAllMap) return;

            self.SetPosition(MiniatureMouseHelper.GetPositionOnWorld());

            if(_lastPosition == self.position || !CanAddOnBoard(self.position)) return;

            _lastPosition = self.position;
            signageUI.Clear();

            _tarrainArea = ScanHelper.ScanFixed(self, stats.width, stats.height, true);
            signageUI.OverlayAttack(_tarrainArea);
        }
    }
}
