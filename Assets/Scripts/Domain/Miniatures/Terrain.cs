using BloodField.Types;
using Helpers;
using BloodField.Managers;
using Render;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using BloodField.Helpers;

namespace BloodField.Miniatures
{
    public class Terrain : Miniature
    {
        private (int y, int x) _lastPosition;
        private List<(int y, int x)> _terrainArea;
        private bool _canApplyEffectToAllMap = true;
        private bool _stopAttach = false;
        private int _turnAmount;
        private (int y, int x) _position;
        private ParticleSystem _vfxInstance;
        private List<GameObject> _floorInstances;

        #region Gets/Sets
        public override bool CanAddOnBoard((int y, int x) position) => GameManager.Instance.mapManager.CanSpawnTerrainUntilMiddleMap(position);
        #endregion

        #region Actions
        private void ApplyDebuff(int multiply = 1)
        {
            _terrainArea?.ForEach(position =>
            {
                var target = GameManager.Instance.mapManager.FindByPosition(position).Find(e => e.IsATarget());

                if (target != null)
                {
                    var miniature = target.gameObject.GetComponent<Miniature>();

                    if (_canApplyEffectToAllMap)
                        TerrainHelper.ApplyDebufferWithConditional(miniature, stats, multiply);
                    else
                        TerrainHelper.ApplyDebufferCommon(miniature, stats, multiply);
                }
            });
        }

        public void Remove()
        {
            ApplyDebuff(-1);
            signageUI?.Clear();
            _terrainArea?.ForEach(position => GameManager.Instance.mapManager.Unregister(TileType.Terrain, position));
            _floorInstances?.ForEach(f => Destroy(f.gameObject));
            Destroy(_vfxInstance);

            Destroy(gameObject, .5f);
        }
        #endregion

        #region Turn
        public override void MyTurn()
        {
            if (!_canApplyEffectToAllMap && _turnAmount >= stats.turnDuration)
                Remove();

            _turnAmount++;
        }
        #endregion

        public override void AddOnBoard((int y, int x) pos)
        {
            signageUI?.Clear();
            _turnAmount = 0;
            _stopAttach = true;

            GameManager.Instance.mapManager.Unregister(self); // remove miniature terrain on map
            GetComponent<SpriteRenderer>().sprite = null;

            if (_canApplyEffectToAllMap)
            {
                GameManager.Instance.matchManager.ChangeTerrainPermanent(this);
                _terrainArea = GameManager.Instance.mapManager.GetPositionsMiddleMap();
            }

            ApplyDebuff();

            _floorInstances = TerrainRender.Render(_terrainArea, gameObject, stats, _canApplyEffectToAllMap);

            stats.customTerrainScript?.Action(_terrainArea, stats, Remove); // call the command specific

            if (stats.effectVFX)
                _vfxInstance = TerrainRender.VfxRender(stats.effectVFX);

            if (IsOwner()) // send message to remote client
            {
                var positionRemote = GameManager.Instance.mapManager.ReflexPosition(_position);
                positionRemote.y += stats.height - 1;

                GameManager.Instance.eventManager.TerrainCreatedEvent(_id, positionRemote, stats, self);
            }
        }

        public void AddOnBoardRemote((int y, int x) pos)
        {
            _terrainArea = ScanHelper.ScanFixed(new Tile(pos.y, pos.x), stats.width, stats.height, true);
            AddOnBoard(pos);
        }


        #region Unity Event
        private void Update()
        {
            if (_canApplyEffectToAllMap || _stopAttach) return;

            _position = MiniatureMouseHelper.GetPositionOnWorld();

            if (_lastPosition == _position || !CanAddOnBoard(_position)) return;

            _lastPosition = _position;
            signageUI?.Clear();

            _terrainArea = ScanHelper.ScanFixed(new Tile(_position.y, _position.x), stats.width, stats.height, true);
            signageUI?.Overlay(_terrainArea, OverlayerType.Terrain, true);
        }
        #endregion

        public override void OnCreate(CardSO card, string ownerId, int y, int x, bool isAttachment)
        {
            base.OnCreate(card, ownerId, y, x, isAttachment);

            GetComponent<SpriteRenderer>().sprite = card.sprite;

            _position = self.position;
            _lastPosition = _position;
            _canApplyEffectToAllMap = stats.canApplyEffectToAllMap;
            _stopAttach = false;
        }
    }
}
