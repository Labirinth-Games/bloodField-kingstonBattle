using Helpers;
using Managers;
using Mirror;
using Render;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Miniatures
{
    public class Miniature : TileElement, ITurn
    {
        [Header("References")]
        [SerializeField] protected GameObject miniaturePreviewHUDPrefab;

        [SerializeField] protected SignageUI signageUI;

        public CardSO stats;
        public string _id { get; private set; } = System.Guid.NewGuid().ToString();
        public bool finishAction
        {
            get => _finishAction;
        }

        protected List<Tile> _tilesToMove = new List<Tile>();
        protected List<Tile> _tilesToAttack = new List<Tile>();
        protected GameObject _instancePreview;

        protected bool _isReady = false;
        protected bool _finishAction = false;
        protected bool _isSelected = false;
        protected int _hp;

        #region Actions
        protected virtual bool Select()
        {
            if (!MiniatureMouseHelper.HasTouchMe(self) || !isOwned) return false;

            signageUI.Clear();
            ToggleSelection();

            if (_isSelected)
            {
                GameManager.Instance.gamePlayManager.SetCurrentMiniature(this);

                _tilesToAttack = ScanHelper.Scan(self, stats.direction, stats.GetD_ATK(), true);
                signageUI.OverlayAttack(_tilesToAttack);

                _tilesToMove = ScanHelper.Scan(self, stats.direction, stats.GetMOV());
                signageUI.OverlayMove(_tilesToMove);

                return true;
            }


            GameManager.Instance.gamePlayManager.SetCurrentMiniature(null);
            return false;
        }

        public virtual void Move((int y, int x) position)
        {
            var tileMove = ScanHelper.CanMoveToTile(_tilesToMove, position);

            if (_finishAction || !_isSelected || tileMove is null || !isOwned) return;

            var pos = self.MoveTo(position);

            CmdMove(new TileSerializerNetwork(self.position));

            FinishAction();
        }

        [Command]
        public void CmdMove(TileSerializerNetwork tile)
        {
            MoveClientRpc(tile);
        }

        [ClientRpc]
        public void MoveClientRpc(TileSerializerNetwork tile)
        {
            var pos = tile.position;
            if (!isOwned)
            {
                pos = GameManager.Instance.mapManager.ReflexPosition(tile.position);
                self.MoveTo(pos);
            }

            transform.position = new Vector3(pos.x, pos.y, 0);
        }

        public virtual void Attack((int y, int x) position)
        {
            Tile enemy = ScanHelper.CanAttackTile(_tilesToAttack, position);

            if (_finishAction || !_isSelected || enemy is null || !isOwned) return;

            if (enemy.gameObject.TryGetComponent(out Miniature miniatureEnemy))
                miniatureEnemy.Hit(stats.GetATK());

            FinishAction();
        }

        public virtual void Hit(int damage)
        {
            _hp -= damage;

            if (_hp <= 0)
                Die();
        }

        public virtual void Die()
        {
            GameManager.Instance.mapManager.Unregister(self);
            GameManager.Instance.miniatureManager.RemoveMiniature(this);
            Destroy(gameObject, .3f);
        }
        #endregion

        #region Gets/Sets
        public virtual void SetReady()
        {
            GameManager.Instance.miniatureManager.AddMiniature(this);
            _isReady = true;
        }

        public virtual bool CanAddOnBoard((int y, int x) position) => true;
        #endregion

        #region Turn Actions
        public virtual void MyTurn()
        {
            _isReady = true;
            _finishAction = false;
            _isSelected = false;
            _hp = stats.GetDEF();

            signageUI.Clear();
        }
        #endregion

        #region Utils
        protected virtual void DestroyPreview()
        {
            if (_instancePreview != null)
                Destroy(_instancePreview);
        }

        protected virtual void FinishAction()
        {
            _finishAction = true;
            signageUI.Clear();

            GameManager.Instance.gamePlayManager.SetCurrentMiniature(null);
            GameManager.Instance.turnManager.SetMiniatureFinishAction();
            _tilesToAttack?.Clear();
            _tilesToMove?.Clear();
        }

        protected virtual void ToggleSelection() => _isSelected = !_isSelected;

        public virtual void VerifyLocalEffect((int y, int x) lastPosition, (int y, int x) currentPosition)
        {
            void Debuff((int y, int x) position, int multiply = 1) =>
                GameManager.Instance.mapManager.FindByPosition(position)
                    .FindAll(tile => tile.IsTerrain())
                    .ForEach(terrain =>
                    {
                        var terrainStats = terrain.gameObject.GetComponent<Miniature>().stats.additionalStats;
                        var myStats = stats.additionalStats;

                        foreach (var stat in terrainStats)
                            myStats[stat.Key] += stat.Value * multiply;
                    });

            Debuff(lastPosition, -1); // verify and remove debuff when exit lastTile
            Debuff(currentPosition); // verify and add the debuff if there is
        }
        #endregion

        #region Mouse Actions
        protected virtual void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0) && _isReady) // left mouse button
            {
                if (_finishAction || GameManager.Instance.gamePlayManager.IsOtherMiniature(_id)) return;

                if (Select()) return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                DestroyPreview();

                _instancePreview = GameManager.Instance.miniatureRender.PreviewRender(stats, _hp, miniaturePreviewHUDPrefab);
            }
        }

        protected virtual void OnMouseExit()
        {
            DestroyPreview();
        }
        #endregion

        #region Network
        public override void OnStartClient()
        {
            base.OnStartClient();
            name = $"{name}-{netId}";
        }

        private void Awake() {
            NetworkClient.RegisterHandler<MiniatureCreateMessage>(OnCreate);
        }
        #endregion

        protected virtual void Subscribers()
        {
            GameManager.Instance.turnManager.OnStartTurnPlayer.AddListener(MyTurn);

            if (self != null)
                self.OnTileMove = VerifyLocalEffect; // add listen when tile move
        }

        public virtual void AddOnBoard((int y, int x) pos)
        {
            self.MoveTo(pos);
            CmdMove(new TileSerializerNetwork(self.position));

            SetReady();
        }

        protected virtual void OnCreate(MiniatureCreateMessage message) { }
    }
}