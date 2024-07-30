using BloodField.Types;
using Helpers;
using BloodField.Managers;
using Render;
using System.Collections.Generic;
using UnityEngine;
using BloodField.Helpers;

namespace BloodField.Miniatures
{
    public class Equipament : Miniature
    {
        private bool _isExcludeActionTurn = false;

        #region Mouse Actions
        protected override void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0) && stats.equipamentType == EquipamentType.Attack) // left mouse button
            {
                if (GameManager.Instance.miniatureManager.IsOtherMiniature(_id) || !_isReady || _isFinishAction) return;

                if (Select()) return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                DestroyPreview();

                _instancePreview = MiniatureRender.PreviewRender(stats, _hp, miniaturePreviewHUDPrefab);
            }
        }

        protected override bool Select()
        {
            if (!MiniatureMouseHelper.HasTouchMe(self)) return false;

            signageUI.Clear();
            ToggleSelection();

            if (_isSelected)
            {
                GameManager.Instance.miniatureManager.SetCurrentMiniature(this);

                _tilesToAttack = ScanHelper.Scan(self, stats.direction, stats.GetD_ATK(), true);
                signageUI.Overlay(_tilesToAttack, OverlayerType.Attack, true);

                return true;
            }

            GameManager.Instance.miniatureManager.SetCurrentMiniature(null);
            return false;
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
            // apply ui when add equipament with status to player show
            if (stats.equipamentType == EquipamentType.Moral)
            {
                var i = 0;

                foreach (var status in stats.additionalStats)
                {
                    if (status.Value != 0)
                    {
                        UIHelper.AdditionalStatsUIRender($"{status.Key} +{status.Value}", gameObject, i);
                        i++;
                    }
                }

                // show message on log
                GameManager.Instance.logHUD.AddMessage(stats.description);
            }

            base.AddOnBoard(pos);
            ApplyEffects();
        }

        public override void Hit(int damage)
        {
            if (stats.isIndestructible)
            {
                UIHelper.HitUIRender("indestructible", gameObject);
                return;
            }

            base.Hit(damage);
        }

        public override void Die()
        {
            base.Die();
            ApplyEffects(-1);
        }

        private void ApplyEffects(int multiply = 1)
        {
            if (stats.equipamentType != EquipamentType.Moral) return;

            /**
            *    Quando for um equipamento de moral ele vai pegar os status e baseado no targets
            *    vai adicionar a todas as miniaturas de exercito que está confugurada assim
            *    adicionando no "additional stats" o aditivo de status para cada miniatura.
            **/
            foreach (var armyType in stats.targets)
            {
                // update all miniatures that already stay on table
                GameManager.Instance.miniatureManager.GetMiniatures()
                    .FindAll(f => f.stats.type == CardType.Army && f.stats.armyType == armyType)
                    .ForEach(miniature =>
                    {
                        foreach (var additionalStats in stats.additionalStats)
                        {
                            miniature.stats.additionalStats[additionalStats.Key] += additionalStats.Value * multiply;

                            if (additionalStats.Key == StatsType.DEF) miniature.AddHP(additionalStats.Value * multiply);
                        }
                    });

                // update reference to new miniatures
                foreach (var additionalStats in stats.additionalStats)
                    GameManager.Instance.miniatureManager.UpdateAddionalStats(armyType, additionalStats.Key, additionalStats.Value * multiply);
            }

        }

        public override void OnCreate(CardSO card, string ownerId, int y, int x, bool isAttachment)
        {
            base.OnCreate(card, ownerId, y, x, isAttachment);

            // remove equipaments of the count to auto finish turn
            List<EquipamentType> excludeTurn = new List<EquipamentType>() { EquipamentType.Moral, EquipamentType.Defense };

            if (excludeTurn.Exists(f => f == stats.equipamentType))
            {
                _isExcludeActionTurn = true;
                _isFinishAction = true;
            }
        }
    }
}
