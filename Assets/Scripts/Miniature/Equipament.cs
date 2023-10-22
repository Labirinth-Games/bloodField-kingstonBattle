using Enums;
using Helpers;
using Managers;
using Render;
using System;
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

        public override void AddOnBoard((int y, int x) pos)
        {
            base.AddOnBoard(pos);
            ApplyEffects();
        }

        public override void Die()
        {
            base.Die();
            ApplyEffects(false);
        }

        private void ApplyEffects(bool isAdd = true)
        {
            if (stats.equipamentType != EquipamentTypeEnum.Moral) return;

            /**
            *    Quando for um equipamento de moral ele vai pegar os status e baseado no targets
            *    vai adicionar a todas as miniaturas de exercito que está confugurada assim
            *    adicionando no "additional stats" o aditivo de status para cada miniatura.
            **/
            foreach (var armyType in stats.targets)
            {
                // update all miniatures that already stay on table
                GameManager.Instance.miniatureManager.GetMiniatures()
                .FindAll(f => f.stats.type == CardTypeEnum.Army && f.stats.armyType == armyType)
                .ForEach(miniature =>
                {
                    foreach (var additionalStats in stats.additionalStats)
                    {
                        var value = isAdd ? additionalStats.Value : additionalStats.Value * -1;
                        miniature.stats.additionalStats[additionalStats.Key] += value;
                    }
                });

                // update reference to new miniatures
                foreach (var additionalStats in stats.additionalStats)
                {
                    var value = isAdd ? additionalStats.Value : additionalStats.Value * -1;
                    GameManager.Instance.gamePlayManager.UpdateAddionalStats(armyType, additionalStats.Key, value);
                }
            }

        }

        public override void Create((int y, int x) pos, CardSO card)
        {
            self = GameManager.Instance.mapManager.Register(new Tile(card.type, gameObject), pos);
            self.SetPositionOnWorld();

            stats = Instantiate(card);
            _hp = stats.GetDEF();

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
