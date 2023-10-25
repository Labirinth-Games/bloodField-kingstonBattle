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
    [SerializeField] private CardSO card;

    #region Turn Actions
    public override void MyTurn()
    {
        _isReady = true;
        _finishAction = false;
        _isSelected = false;

        signageUI.Clear();
    }
    #endregion

    public override void Create((int y, int x) pos)
    {
        self = GameManager.Instance.mapManager.Register(new Tile(TileTypeEnum.King, gameObject), pos);
        self.SetPositionOnWorld();
        SetReady();

        stats = Instantiate(card);
        _hp = stats.GetDEF();

        Subscribers();
    }
}
