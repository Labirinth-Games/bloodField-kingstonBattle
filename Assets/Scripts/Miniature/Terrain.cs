using Enums;
using Helpers;
using Managers;
using Render;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using Unity.VisualScripting;
using UnityEngine;

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

                    foreach (var terrainStats in stats.additionalStats)
                        targetStats[terrainStats.Key] += terrainStats.Value * multiply; // mulyiply is used to add or remove value added
                }
            });
        }

        private void Remove()
        {
            ApplyDebuff(-1);
            signageUI.Clear();
            _tarrainArea.ForEach(position => GameManager.Instance.mapManager.Unregister(TileTypeEnum.Terrain, position));
            _floorInstances.ForEach(f => Destroy(f.gameObject));
            Destroy(_vfxInstance);

            Destroy(gameObject, .2f);
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

        public override void OnCreate(MiniatureCreateMessage miniature)
        {
            self = GameManager.Instance.mapManager.Register(new Tile(CardTypeEnum.Terrain, gameObject), miniature.position);
            self.SetPositionOnWorld();

            stats = Instantiate(miniature.card);
            _position = self.position;
            _lastPosition = _position;
            _canApplyEffectToAllMap = stats.canApplyEffectToAllMap;
            _stopAttach = false;

            Subscribers();
        }

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
            signageUI.OverlayAttack(_tarrainArea);
        }
        #endregion
    }
}
