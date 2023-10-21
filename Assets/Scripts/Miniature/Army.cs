using Helpers;
using Managers;
using Render;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using UnityEngine.UIElements;

namespace Miniatures
{
    public class Army : Miniature
    {
        #region Mouse Actions
        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0)) // left mouse button
            {
                if (!_isReady || _finishAction) return;

                var position = MiniatureMouseHelper.GetPositionOnWorld();

                Select();
                Move(position);
                Attack(position);

                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                DestroyPreview();

                _instancePreview = MiniatureRender.PreviewRender(stats, miniaturePreviewHUDPrefab);
            }
        }

        private void OnMouseExit()
        {
            DestroyPreview();
        }
        #endregion

        #region Gets/Sets
        public override bool CanAddOnBoard((int y, int x) position) => GameManager.Instance.mapManager.CanSpawnMiniatures(position);
        #endregion

        public override void Create((int y, int x) pos, CardSO card)
        {
            self = GameManager.Instance.mapManager.Register(new Tile(card.type, gameObject), pos);
            self.SetPositionOnWorld();

            stats = Instantiate(card);
            _fullHP = stats.DEF;
        }
    }
}
