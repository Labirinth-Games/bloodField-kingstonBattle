using Enums;
using Helpers;
using BloodField.Managers;
using Miniatures;
using Render;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

public class King : Miniature
{
    #region Turn Actions
    // public override void MyTurn()
    // {
    //     _isReady = true;
    //     _isFinishAction = false;
    //     _isSelected = false;

    //     signageUI.Clear();
    // }
    #endregion

    public override void OnCreate(CardSO card, string ownerId, int y, int x)
    {
        var pos = GameManager.Instance.mapManager.GetKingPositions();
        _ownerId = ownerId;

        if (!IsOwner())
            pos = GameManager.Instance.mapManager.ReflexPosition(pos);

        self = GameManager.Instance.mapManager.Register(new Tile(TileTypeEnum.King, gameObject), pos);
        self.SetPositionOnWorld();
        SetReady();
        Subscribers();

        stats = Instantiate(card);
        _hp = stats.GetDEF();
    }
}
