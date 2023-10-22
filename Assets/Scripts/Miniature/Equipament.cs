using Enums;
using Helpers;
using Managers;
using Render;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Miniatures
{
    public class Equipament : Miniature
    {
        private bool _isExcludeActionTurn = false;

        #region Mouse Actions
        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0) && stats.equipamentType == EquipamentTypeEnum.Attack) // left mouse button
            {
                if (!_isReady || _finishAction) return;

                var position = MiniatureMouseHelper.GetPositionOnWorld();

                Select(true, false);
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
        public override bool CanAddOnBoard((int y, int x) position) => GameManager.Instance.mapManager.CanSpawnUntilMiddleMiniatures(position);
        #endregion

        public override void MyTurn()
        {
            if (_isExcludeActionTurn) return;
            base.MyTurn();
        }

        public override void Create((int y, int x) pos, CardSO card)
        {
            self = GameManager.Instance.mapManager.Register(new Tile(card.type, gameObject), pos);
            self.SetPositionOnWorld();

            stats = Instantiate(card);
            _fullHP = stats.DEF;

            // remove equipaments of the count to auto finish turn
            List<EquipamentTypeEnum> excludeTurn = new List<EquipamentTypeEnum>() { EquipamentTypeEnum.Moral, EquipamentTypeEnum.Defense };

            if (excludeTurn.Exists(f => f == stats.equipamentType)) 
            {
                _isExcludeActionTurn = true;
                _finishAction = true;
            }
        }
    }
}
