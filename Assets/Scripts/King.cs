using Enums;
using Helpers;
using Managers;
using Miniatures;
using Render;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

public class King : Miniature
{
    [SerializeField] private Sprite kingSprite;
    [SerializeField] private CardSO card;

    #region Mouse Actions
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) // left mouse button
        {
            if (!_isReady || _finishAction) return;

            var position = MiniatureMouseHelper.GetPositionOnWorld();

            Select();
            Move(position);
            Attack(position);

            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            DestroyPreview();

            _instancePreview = MiniatureRender.PreviewRender(stats, miniaturePreviewHUDPrefab);
        }
    }

    private void OnMouseExit()
    {
        DestroyPreview();
    }
    #endregion

    public override void Create((int y, int x) pos)
    {
        self = GameManager.Instance.mapManager.Register(new Tile(TileTypeEnum.King, gameObject), pos);
        self.SetPositionOnWorld();
        SetReady();

        GetComponent<SpriteRenderer>().sprite = kingSprite;

        stats = Instantiate(card);
        stats.sprite = kingSprite;
        _fullHP = stats.DEF;
    }
}
