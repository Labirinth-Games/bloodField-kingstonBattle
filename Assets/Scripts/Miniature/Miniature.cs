using Helpers;
using Managers;
using Render;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Tiles;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Miniatures
{
    public class Miniature : TileElement, ITurn
    {
        [Header("References")]
        [SerializeField] protected GameObject miniaturePreviewHUDPrefab;

        [SerializeField] protected SignageUI signageUI;

        public CardSO stats { get; protected set; }
        public bool finishAction
        {
            get => _finishAction;
        }

        protected List<Tile> _tilesToMove;
        protected List<Tile> _tilesToAttack;
        protected GameObject _instancePreview;

        protected bool _isReady = false;
        protected bool _finishAction = false;
        protected bool _isSelected = false;
        protected int _fullHP;

        #region Actions
        protected virtual void Select(bool showAttackUI = true, bool showMoveUI = true)
        {
            if (!MiniatureMouseHelper.HasTouchMe(self)) return;

            ToggleSelection();

            if (!_isSelected)
            {
                signageUI.Clear();
                GetComponent<BoxCollider2D>().size = Vector2.one;
                return;
            }

            GameManager.Instance.gamePlayManager.SetCurrentMiniature(this);

            signageUI.Clear();

            if(showAttackUI)
            {
                _tilesToAttack = ScanHelper.Scan(self, stats.direction, stats.D_ATK, true);
                signageUI.OverlayAttack(_tilesToAttack);
            }

            if(showMoveUI)
            {
                _tilesToMove = ScanHelper.Scan(self, stats.direction, stats.MOV);
                signageUI.OverlayMove(_tilesToMove);
            }


            GetComponent<BoxCollider2D>().size *= (stats.D_ATK + stats.MOV) * 2;
        }

        protected virtual void Move((int y, int x) position)
        {
            if (!ScanHelper.CanMoveToTile(_tilesToMove, position)
              || !GameManager.Instance.mapManager.FindByPosition(position).Exists(f => f.CanMove())
              || _finishAction)
                return;

            self.MoveTo(position);

            FinishAction();
        }

        protected virtual void Attack((int y, int x) position)
        {
            if (!ScanHelper.CanMoveToTile(_tilesToAttack, position) || _finishAction) return;

            var enemy = GameManager.Instance.mapManager.FindByPosition(position).Find(f => f.AnyElement());

            if (enemy != null && enemy.gameObject.TryGetComponent(out Miniature miniatureEnemy))
            {
                miniatureEnemy.Hit(stats.ATK);
            }

            FinishAction();
        }

        public virtual void Hit(int damage)
        {
            stats.DEF -= damage;

            if (stats.DEF <= 0)
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

        public virtual bool CanAddOnBoard((int y, int x) position) { return true; }
        #endregion

        #region Turn Actions
        public virtual void MyTurn()
        {
            _isReady = true;
            _finishAction = false;
            _isSelected = false;
            stats.DEF = _fullHP;

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

            GetComponent<BoxCollider2D>().size = Vector2.one;
            GameManager.Instance.gamePlayManager.SetCurrentMiniature(null);
            GameManager.Instance.turnManager.SetMiniatureFinishAction();
        }

        protected virtual void ToggleSelection() => _isSelected = !_isSelected;
        #endregion

        public virtual void Create((int y, int x) pos, CardSO card) 
        {
            stats = Instantiate(card);
        }
        
        public virtual void AddOnBoard((int y, int x) pos)
        {
            self.MoveTo(pos);
            SetReady();
        }
    }
}