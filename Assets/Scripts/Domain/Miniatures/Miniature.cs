using DG.Tweening;
using Helpers;
using BloodField.Managers;
using Render;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using Nakama;
using BloodField.Types;
using System.Text;
using Nakama.TinyJson;
using BloodField.Helpers;
using BloodField.Network.Entities;
using System.Threading.Tasks;

namespace BloodField.Miniatures
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
            get => _isFinishAction;
        }

        protected List<Tile> _tilesToMove = new List<Tile>();
        protected List<Tile> _tilesToAttack = new List<Tile>();
        protected GameObject _instancePreview;

        protected bool _isReady = false;
        protected bool _isFinishAction = false;
        protected bool _isSelected = false;
        protected int _hp;
        protected string _ownerId;

        protected bool IsOwner() => _ownerId == GameManager.Instance.UserId;

        #region Actions
        protected virtual bool Select()
        {
            if (!MiniatureMouseHelper.HasTouchMe(self) || !IsOwner()) return false;

            signageUI.Clear();
            ToggleSelection();

            if (_isSelected)
            {
                GameManager.Instance.miniatureManager.SetCurrentMiniature(this);

                _tilesToAttack = ScanHelper.Scan(self, stats.direction, stats.GetD_ATK(), true);
                signageUI.Overlay(_tilesToAttack, OverlayerType.Attack, true);

                _tilesToMove = ScanHelper.Scan(self, stats.direction, stats.GetMOV());
                signageUI.Overlay(_tilesToMove, OverlayerType.Move);

                return true;
            }

            GameManager.Instance.miniatureManager.SetCurrentMiniature(null);
            return false;
        }

        public virtual async void Move((int y, int x) position)
        {
            var tileMove = ScanHelper.CanMoveToTile(_tilesToMove, position);

            if (_isFinishAction || !_isSelected || tileMove is null || !IsOwner()) return;

            var pos = self.MoveTo(position);

            await NetworkHelper.Send<MiniatureNetworkEntity>(
                OpCodeType.MINIATURE_MOVE,
                new MiniatureNetworkEntity
                {
                    x = (int)pos.x,
                    y = (int)pos.y
                }
            );

            FinishAction();
        }

        public virtual void Attack((int y, int x) position)
        {
            Tile enemy = ScanHelper.CanAttackTile(_tilesToAttack, position);

            if (_isFinishAction || !_isSelected || enemy is null || !IsOwner()) return;

            if (enemy.gameObject.TryGetComponent(out Miniature miniatureEnemy))
                miniatureEnemy.Hit(stats.GetATK());

            FinishAction();
        }

        public virtual void Hit(int damage)
        {
            _hp -= damage;

            transform.DOScale(1.4f, .1f).SetLoops(2, LoopType.Yoyo);
            UIHelper.HitUIRender($"-{damage}", gameObject);

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

        public void SetInactive() => _isFinishAction = true;
        public void SetActive() => _isFinishAction = true;
        public void AddHP(int val)
        {
            _hp += val;

            if (_hp <= 0)
            {
                transform.DOScale(1.4f, .1f).SetLoops(2, LoopType.Yoyo);
                Die();
            }
        }
        public virtual bool CanAddOnBoard((int y, int x) position) => true;
        #endregion

        #region Turn Actions
        public virtual void MyTurn()
        {
            _isReady = true;
            _isFinishAction = false;
            _isSelected = false;

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
            _isFinishAction = true;
            signageUI.Clear();

            GameManager.Instance.miniatureManager.SetCurrentMiniature(null);
            GameManager.Instance.turnManager.SetMiniatureFinishAction();
            _tilesToAttack?.Clear();
            _tilesToMove?.Clear();
        }

        protected virtual void ToggleSelection() => _isSelected = !_isSelected;

        public virtual async void VerifyLocalEffect((int y, int x) lastPosition, (int y, int x) currentPosition)
        {
            void Debuff((int y, int x) position, int multiply = 1) =>
                GameManager.Instance.mapManager.FindByPosition(position)
                    .FindAll(tile => tile.IsTerrain())
                    .ForEach(terrain =>
                    {
                        var terrainStats = terrain.gameObject.GetComponent<Miniature>().stats;

                        if (terrainStats.canApplyEffectToAllMap)
                            TerrainHelper.ApplyDebufferWithConditional(this, terrainStats, multiply);
                        else
                            TerrainHelper.ApplyDebufferCommon(this, terrainStats, multiply);
                    });

            Debuff(lastPosition, -1); // verify and remove debuff when exit lastTile
            await Task.Delay(200);
            Debuff(currentPosition); // verify and add the debuff if there is
        }
        #endregion

        #region Network Events
        private void OnReceivedMatchState(IMatchState matchState)
        {
            var jsonUtf8 = Encoding.UTF8.GetString(matchState.State);
            var content = JsonParser.FromJson<Dictionary<string, string>>(jsonUtf8);
            var isOwner = content.ContainsKey("userId") && content["userId"] == GameManager.Instance.UserId;

            switch (matchState.OpCode)
            {
                case OpCodeType.MINIATURE_MOVE:
                    if (!isOwner)
                    {
                        MiniatureNetworkEntity miniature = JsonParser.FromJson<MiniatureNetworkEntity>(jsonUtf8);

                        if (miniature.id == _id)
                        {
                            var pos = GameManager.Instance.mapManager.ReflexPosition((miniature.y, miniature.x));
                            self.MoveTo(pos);
                        }
                    }
                    break;
            }
        }
        #endregion

        #region Mouse Actions
        protected virtual void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0) && _isReady && GameManager.Instance.turnManager.IsMyTurn() && GameManager.Instance.matchManager.IsMainPhase()) // left mouse button
            {
                if (_isFinishAction || GameManager.Instance.miniatureManager.IsOtherMiniature(_id)) return;

                if (Select()) return;
            }

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
            GameManager.Instance.eventManager.OnStartMyTurn += MyTurn;
            GameManager.Instance.eventManager.OnReceivedMatchState += OnReceivedMatchState;

            if (self != null)
                self.OnTileMove = VerifyLocalEffect; // add listen when tile move
        }

        public virtual void AddOnBoard((int y, int x) pos)
        {
            self.MoveTo(pos);
            SetReady();
        }

        void OnDestroy()
        {
            GameManager.Instance.eventManager.OnStartMyTurn -= MyTurn;
            GameManager.Instance.eventManager.OnReceivedMatchState -= OnReceivedMatchState;
        }

        public virtual void OnCreate(CardSO card, string ownerId, int y, int x, bool isAttachment)
        {
            // create tile config
            self = GameManager.Instance.mapManager.Register(new Tile(card.type, gameObject), (y, x));
            GetComponent<SpriteRenderer>().sprite = card.sprite;

            _ownerId = ownerId;

            // setting stats
            stats = Instantiate(card);
            _hp = stats.GetDEF();

            if (!GameManager.Instance.turnManager.IsMyTurn())
                _isFinishAction = true;

            Subscribers();

            // attachment the army on mouse to set position
            if (isAttachment)
                GameManager.Instance.miniatureMouseHelper.Attachment(gameObject);
        }
    }
}