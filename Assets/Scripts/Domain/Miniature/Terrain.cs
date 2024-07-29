using BloodField.Types;
using Helpers;
using BloodField.Managers;
using Render;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using BloodField.Helpers;
using System;

namespace Miniatures
{
    public class Terrain : Miniature
    {
        private (int y, int x) _lastPosition;
        private List<(int y, int x)> _tarrainArea;
        private bool _canApplyEffectToAllMap = true;
        private bool _stopAttach = false;
        private int _turnAmount;
        private (int y, int x) _position;
        private ParticleSystem _vfxInstance;
        private List<GameObject> _floorInstances;

        #region Gets/Sets
        public override bool CanAddOnBoard((int y, int x) position) => GameManager.Instance.mapManager.CanSpawnUntilMiddleMiniatures(position);
        #endregion

        #region Actions
        private void ApplyDebuff(int multiply = 1)
        {
            _tarrainArea.ForEach(position =>
            {
                var target = GameManager.Instance.mapManager.FindByPosition(position).Find(e => e.IsATarget());

                if (target != null)
                {
                    var targetStats = target.gameObject.GetComponent<Miniature>().stats.additionalStats;
                    int i = 0;

                    foreach (var terrainStats in stats.additionalStats)
                    {
                        targetStats[terrainStats.Key] += terrainStats.Value * multiply; // mulyiply is used to add or remove value added
                        UIHelper.AdditionalStatsUIRender($"{terrainStats.Key} {(multiply < 0 ? "+" : "-")}{Math.Abs(terrainStats.Value)}", target.gameObject, i);
                        i++;
                    }
                }
            });
        }

        private void Remove()
        {
            ApplyDebuff(-1);
            signageUI.Clear();
            _tarrainArea.ForEach(position => GameManager.Instance.mapManager.Unregister(TileType.Terrain, position));
            _floorInstances.ForEach(f => Destroy(f.gameObject));
            Destroy(_vfxInstance);

            Destroy(gameObject, .5f);
        }
        #endregion

        #region Turn
        public override void MyTurn()
        {
            if (_turnAmount >= stats.turnDuration)
                Remove();

            _turnAmount++;
        }
        #endregion

        public override void AddOnBoard((int y, int x) pos)
        {
            signageUI.Clear();
            _turnAmount = 0;
            _stopAttach = true;
            GameManager.Instance.mapManager.Unregister(self); // remove miniature terrain on map
            GetComponent<SpriteRenderer>().sprite = null;

            ApplyDebuff();

            _floorInstances = TerrainRender.Render(_tarrainArea, gameObject, stats.effectSprite);
            _vfxInstance = TerrainRender.VfxRender(stats.effectVFX);
        }

        #region Unity Event
        private void Update()
        {
            if (_canApplyEffectToAllMap || _stopAttach) return;

            _position = MiniatureMouseHelper.GetPositionOnWorld();

            if (_lastPosition == _position || !CanAddOnBoard(_position)) return;

            _lastPosition = _position;
            signageUI.Clear();

            _tarrainArea = ScanHelper.ScanFixed(new Tile(_position.y, _position.x), stats.width, stats.height, true);
            signageUI.Overlay(_tarrainArea, OverlayerType.Terrain, true);
        }
        #endregion

        public override void OnCreate(CardSO card, string ownerId, int y, int x)
        {
            base.OnCreate(card, ownerId, y, x);

            _position = self.position;
            _lastPosition = _position;
            _canApplyEffectToAllMap = stats.canApplyEffectToAllMap;
            _stopAttach = false;
        }
    }
}
