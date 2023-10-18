using Helpers;
using Managers;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Tiles;
using UnityEngine;

public class Miniature : TileElement
{
    [SerializeField] private SignageUI signageUI;

    public CardSO stats { get; private set; }

    private List<Tile> _tilesToMove;
    private List<Tile> _tilesToAttack;
    private bool _isReady = false;
    private bool _finishAction = false;

    #region Actions
    private void Select()
    {
        if (GameManager.Instance.gamePlayManager.IsOtherMiniature()) return;

        GameManager.Instance.gamePlayManager.SetCurrentMiniature(this);

        signageUI.Clear();

        _tilesToMove = ScanHelper.Scan(self, stats.direction, stats.MOV);
        _tilesToAttack = ScanHelper.Scan(self, stats.direction, stats.D_ATK);

        signageUI.OverlayAttack(_tilesToAttack);
        signageUI.OverlayMove(_tilesToMove);

        GetComponent<BoxCollider2D>().size *= (stats.D_ATK + stats.MOV) * 2;
    }

    private void Move((int y, int x) position)
    {
        if (!ScanHelper.CanMoveToTile(_tilesToMove, position)) return;

        self.MoveTo(position);

        _finishAction = true;
        signageUI.Clear();

        GetComponent<BoxCollider2D>().size = Vector2.one;
        GameManager.Instance.gamePlayManager.SetCurrentMiniature(null);
    }

    private void Attack((int y, int x) position)
    {
        if (ScanHelper.CanMoveToTile(_tilesToAttack, position))
        {
            Debug.Log("A takes");
        }
    }
    #endregion

    private void OnMouseDown()
    {
        if (!_isReady || _finishAction) return;

        var position = MiniatureMouseHelper.GetPositionOnWorld();

        if (position == self.position)
        {
            Select();
            return;
        }

        Move(position);
        Attack(position);
    }

    #region Gets/Sets
    public void SetReady() 
    {
        GameManager.Instance.gamePlayManager.AddMiniature(this);
        _isReady = true;
    }
    #endregion

    public void Create((int y, int x) pos, Card card)
    {
        self = GameManager.Instance.mapManager.Register(new Tile(card.stats.type, gameObject), pos);
        self.SetPositionOnWorld();

        stats = Instantiate(card.stats);
    }
}
