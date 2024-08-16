using BloodField.Types;
using BloodField.Managers;
using Tiles;
using UnityEngine;
using BloodField.Miniatures;
using Helpers;

public class King : Miniature
{
    public override void OnCreate(CardSO card, string ownerId, int y, int x, bool isAttachment)
    {
        var pos = GameManager.Instance.mapManager.GetKingPositions();
        _ownerId = ownerId;

        self = GameManager.Instance.mapManager.Register(new Tile(TileType.King, gameObject), pos);
        self.SetPositionOnWorld();

        stats = Instantiate(card);
        _hp = stats.GetDEF();
        
        GetComponent<SpriteRenderer>().sprite = SpriteColorDynamic.ChangeColorBase(card.sprite, stats.color);
        
        SetReady();
        Subscribers();
    }

    public override void Die()
    {
        base.Die();

        GameManager.Instance.eventManager.EndGameEvent(false);
    }
}
