using System.Collections.Generic;
using BloodField.Managers;
using BloodField.Types;
using Helpers;
using Render;
using Tiles;
using UnityEngine;

namespace BloodField.Miniatures
{
    public class Army : Miniature
    {
        private (int y, int x) _lastPosition = (0, 0);
        private bool _isOnBoard = false;
        private List<Tile> _groupPositions;

        #region Gets/Sets
        public override bool CanAddOnBoard((int y, int x) position) => GameManager.Instance.mapManager.CanSpawnMiniatures(position);
        #endregion

        #region Utils
        protected void ApplyAdditionalStats()
        {
            // TODO - validar cenário quando uma miniature é criada em cima de um terreno ja existente
            GameManager.Instance.miniatureManager.GetAdditionalStats()
                .FindAll(f => f.type == stats.armyType)
                .ForEach(additionalStats =>
                {
                    foreach (var stat in additionalStats.stats)
                    {
                        stats.additionalStats[stat.Key] = stat.Value;
                        if (stat.Key == BloodField.Types.StatsType.DEF) AddHP(stat.Value);
                    }
                });
        }
        #endregion

        public override void AddOnBoard((int y, int x) pos)
        {
            base.AddOnBoard(pos);
            if (stats.isGroup) AddArmyGroupOnBoard();
        }

        #region Group Behavior
        private void AddArmyGroupOnBoard()
        {
            _isOnBoard = true;
            signageUI.Clear();

            foreach (var element in _groupPositions)
            {
                var cloneStats = Instantiate(stats);
                cloneStats.isGroup = false;

                var instance = MiniatureRender.Render(cloneStats, Resources.Load<GameObject>("Miniatures/ArmyMiniaturePrefab"), element.position, false);
                var army = instance.GetComponent<Army>();
                army.self.MoveTo(element.position);
                army.SetReady();

                instance.transform.position = new Vector3(element.position.x, element.position.y, 0);

                GameManager.Instance.eventManager.MiniatureCreatedEvent(army._id, army.stats, army.self);
            }
        }

        private void Update()
        {
            if (!stats.isGroup || _isOnBoard) return;

            var _position = MiniatureMouseHelper.GetPositionOnWorld();

            if (_lastPosition == _position || !CanAddOnBoard(_position)) return;

            _lastPosition = _position;
            signageUI.Clear();

            _groupPositions = ScanHelper.Scan(new Tile(_position.y, _position.x), ScanDirectionType.Horizontal_Line, 1, false);
            signageUI.Overlay(_groupPositions, OverlayerType.Terrain, true);
        }
        #endregion

        public override void OnCreate(CardSO card, string ownerId, int y, int x, bool isAttachment)
        {
            base.OnCreate(card, ownerId, y, x, isAttachment);
            ApplyAdditionalStats();

            // GetComponent<SpriteRenderer>().sprite = SpriteColorDynamic.ChangeColorBase(GetComponent<SpriteRenderer>().sprite, new SpriteColors() { primary = stats.primaryColor, secundary = stats.secundaryColor });
        }
    }
}
