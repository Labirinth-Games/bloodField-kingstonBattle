using DG.Tweening;
using Helpers;
using BloodField.Managers;
using Render;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using Nakama;
using BloodField.Types;
using BloodField.Helpers;
using BloodField.Network.Entities;

namespace BloodField.Miniatures
{
    public class MiniatureRemote : TileElement
    {
        [Header("References")]
        [SerializeField] protected GameObject miniaturePreviewHUDPrefab;
        [SerializeField] protected SignageUI signageUI;

        public CardSO stats;

        protected List<Tile> _tilesToMove = new List<Tile>();
        protected List<Tile> _tilesToAttack = new List<Tile>();
        protected GameObject _instancePreview;

        protected bool _isSelected = false;
        protected int _hp;
        protected string _ownerId;
        private string _id;

        protected bool IsOwner() => _ownerId == GameManager.Instance.UserId;

        #region Actions
        protected virtual bool Select()
        {
            if (!MiniatureMouseHelper.HasTouchMe(self) || !IsOwner()) return false;

            signageUI.Clear();
            ToggleSelection();

            if (_isSelected)
            {
                _tilesToAttack = ScanHelper.Scan(self, stats.direction, stats.GetD_ATK(), true);
                signageUI.Overlay(_tilesToAttack, OverlayerType.Attack, true);

                _tilesToMove = ScanHelper.Scan(self, stats.direction, stats.GetMOV());
                signageUI.Overlay(_tilesToMove, OverlayerType.Move);

                return true;
            }

            return false;
        }

        public virtual async void Hit(int damage)
        {
            _hp -= damage;

            transform.DOScale(1.4f, .1f).SetLoops(2, LoopType.Yoyo);
            UIHelper.HitUIRender($"-{damage}", gameObject);

            if (_hp <= 0)
                Die();

            await NetworkHelper.Send<MiniatureNetworkEntity>(OpCodeType.MINIATURE_HIT, new MiniatureNetworkEntity() { damage = damage, id = _id });
        }

        public virtual async void Die()
        {
            GameManager.Instance.mapManager.Unregister(self);
            Destroy(gameObject, .3f);

            await NetworkHelper.Send<MiniatureNetworkEntity>(OpCodeType.MINIATURE_DEATH, new MiniatureNetworkEntity() { id = _id });
        }
        #endregion

        #region Gets/Sets

        public void AddHP(int val)
        {
            _hp += val;

            if (_hp <= 0)
            {
                transform.DOScale(1.4f, .1f).SetLoops(2, LoopType.Yoyo);
                Die();
            }
        }
        #endregion

        #region Utils
        protected virtual void DestroyPreview()
        {
            if (_instancePreview != null)
                Destroy(_instancePreview);
        }

        protected virtual void ToggleSelection() => _isSelected = !_isSelected;
        #endregion

        #region Network Events
        private void OnReceivedMatchState(IMatchState matchState)
        {
            NetworkHelper.Listen<MiniatureNetworkEntity>(matchState, OpCodeType.MINIATURE_MOVE, (content, isHost, isOwner) =>
            {
                if (isOwner || _id != content.id) return;

                var pos = GameManager.Instance.mapManager.ReflexPosition(content.GetPosition());
                self.MoveTo(pos);
            });
        }
        #endregion

        #region Mouse Actions
        protected virtual void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1))
            {
                DestroyPreview();

                _instancePreview = MiniatureRender.PreviewRender(stats, _hp, miniaturePreviewHUDPrefab);
            }
        }

        protected virtual void OnMouseExit()
        {
            DestroyPreview();
        }
        #endregion

        protected virtual void Subscribers()
        {
            GameManager.Instance.eventManager.OnReceivedMatchState += OnReceivedMatchState;
        }

        void OnDestroy()
        {
            GameManager.Instance.eventManager.OnReceivedMatchState -= OnReceivedMatchState;
        }

        public virtual void OnCreate(string id, CardSO card, string ownerId, int y, int x)
        {
            // create tile config
            self = GameManager.Instance.mapManager.Register(new Tile(card.type, gameObject), (y, x));

            transform.position = new Vector3(x, y, 0);

            _ownerId = ownerId;
            _id = id;

            // setting stats
            stats = Instantiate(card);
            _hp = stats.GetDEF();

            if (new List<CardType> { CardType.Equipament, CardType.Army, CardType.King }.Exists(e => e == stats.type))
                GetComponent<SpriteRenderer>().sprite = SpriteColorDynamic.ChangeColorBase(card.sprite, stats.color, true);

            Subscribers();
        }
    }
}