using DG.Tweening;
using Enums;
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

        public CardSO stats;
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
        protected int _hp;

        #region Actions
        protected virtual void Select(bool showAttackUI = true, bool showMoveUI = true)
        {
            if (!MiniatureMouseHelper.HasTouchMe(self)) return;
            if (GameManager.Instance.gamePlayManager.IsOtherMiniature()) return;

            ToggleSelection();

            if (!_isSelected)
            {
                signageUI.Clear();
                GameManager.Instance.gamePlayManager.SetCurrentMiniature(null);
                GetComponent<BoxCollider2D>().size = Vector2.one;
                return;
            }

            GameManager.Instance.gamePlayManager.SetCurrentMiniature(this);

            signageUI.Clear();

            if (showAttackUI)
            {
                _tilesToAttack = ScanHelper.Scan(self, stats.direction, stats.GetD_ATK(), true);
                signageUI.OverlayAttack(_tilesToAttack);
            }

            if (showMoveUI)
            {
                _tilesToMove = ScanHelper.Scan(self, stats.direction, stats.GetMOV());
                signageUI.OverlayMove(_tilesToMove);
            }


            GetComponent<BoxCollider2D>().size *= (stats.GetD_ATK() + stats.GetMOV()) * 2;
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
                miniatureEnemy.Hit(stats.GetATK());
            }

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

        public virtual bool CanAddOnBoard((int y, int x) position) { return true; }
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

            GetComponent<BoxCollider2D>().size = Vector2.one;
            GameManager.Instance.gamePlayManager.SetCurrentMiniature(null);
            GameManager.Instance.turnManager.SetMiniatureFinishAction();
            _tilesToAttack?.Clear();
            _tilesToMove?.Clear();
        }

        protected virtual void ToggleSelection() => _isSelected = !_isSelected;
        #endregion

        public virtual void Create((int y, int x) pos, CardSO card)
        {
            stats = Instantiate(card);
        }

        public virtual void Create((int y, int x) pos) { }

        public virtual void AddOnBoard((int y, int x) pos)
        {
            self.MoveTo(pos);
            SetReady();
        }
    }
}