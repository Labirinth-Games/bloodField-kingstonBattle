using Helpers;
using Managers;
using Render;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Tiles;
using UnityEngine;
using UnityEngine.EventSystems;

public class Miniature : TileElement, ITurn
{
    [Header("References")]
    [SerializeField] private GameObject miniaturePreviewPrefab;

    [SerializeField] private SignageUI signageUI;

    public CardSO stats { get; private set; }
    public bool finishAction
    {
        get => _finishAction;
    }

    private List<Tile> _tilesToMove;
    private List<Tile> _tilesToAttack;
    private GameObject _instancePreview;
    private bool _isReady = false;
    private bool _finishAction = false;
    private bool _isSelected = false;
    private int _fullHP;

    #region Actions
    private void Select()
    {
        if (_isSelected)
        {
            signageUI.Clear();
            GetComponent<BoxCollider2D>().size = Vector2.one;
            return;
        }

        GameManager.Instance.gamePlayManager.SetCurrentMiniature(this);

        signageUI.Clear();

        _tilesToMove = ScanHelper.Scan(self, stats.direction, stats.MOV);
        _tilesToAttack = ScanHelper.Scan(self, stats.direction, stats.D_ATK, true);

        signageUI.OverlayAttack(_tilesToAttack);
        signageUI.OverlayMove(_tilesToMove);

        GetComponent<BoxCollider2D>().size *= (stats.D_ATK + stats.MOV) * 2;

        _isSelected = !_isSelected;
    }

    private void Move((int y, int x) position)
    {
        if (!ScanHelper.CanMoveToTile(_tilesToMove, position)) return;

        self.MoveTo(position);

        FinishAction();
    }

    private void Attack((int y, int x) position)
    {
        if (!ScanHelper.CanMoveToTile(_tilesToAttack, position)) return;

        var enemy = GameManager.Instance.mapManager.FindByPosition(position).Find(f => f.AnyElement());

        if (enemy.gameObject.TryGetComponent(out Miniature miniatureEnemy))
        {
            miniatureEnemy.Hit(stats.ATK);
        }

        FinishAction();
    }

    public void Hit(int damage)
    {
        stats.DEF -= damage;

        if (stats.DEF <= 0)
            Die();
    }

    public void Die()
    {
        GameManager.Instance.mapManager.Unregister(self);
        Destroy(gameObject, .3f);
    }
    #endregion

    #region Mouse Actions
    private void OnMouseOver()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (!_isReady || _finishAction) return;

            //if (GameManager.Instance.gamePlayManager.IsOtherMiniature()) return;

            var position = MiniatureMouseHelper.GetPositionOnWorld();

            if (position == self.position)
            {
                Select();
                return;
            }

            if (GameManager.Instance.mapManager.FindByPosition(position).Exists(f => f.IsMove()))
            {
                Move(position);
                return;
            }

            Attack(position);
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            DestroyPreview();

            _instancePreview = MiniatureRender.PreviewRender(stats, miniaturePreviewPrefab);
        }
    }

    private void OnMouseExit()
    {
        DestroyPreview();
    }
    #endregion

    #region Gets/Sets
    public void SetReady()
    {
        GameManager.Instance.gamePlayManager.AddMiniature(this);
        _isReady = true;
    }
    #endregion

    #region Turn Actions
    public void MyTurn()
    {
        _isReady = true;
        _finishAction = false;
        _isSelected = false;
        stats.DEF = _fullHP;

        signageUI.Clear();
    }
    #endregion

    #region Utils
    private void DestroyPreview()
    {
        if (_instancePreview != null)
            Destroy(_instancePreview);
    }

    private void FinishAction()
    {
        _finishAction = true;
        signageUI.Clear();

        GetComponent<BoxCollider2D>().size = Vector2.one;
        GameManager.Instance.gamePlayManager.SetCurrentMiniature(null);
        GameManager.Instance.turnManager.SetMiniatureFinishAction();
    }
    #endregion

    public void Create((int y, int x) pos, CardSO card)
    {
        self = GameManager.Instance.mapManager.Register(new Tile(card.type, gameObject), pos);
        self.SetPositionOnWorld();

        stats = Instantiate(card);
        _fullHP = stats.DEF;
    }
}
