using BloodField.Types;
using BloodField.Managers;
using Miniatures;
using Tiles;
using UnityEngine;

public class King : Miniature
{
    public override void OnCreate(CardSO card, string ownerId, int y, int x)
    {
        var pos = GameManager.Instance.mapManager.GetKingPositions();
        _ownerId = ownerId;

        if (!IsOwner())
            pos = GameManager.Instance.mapManager.ReflexPosition(pos);

        self = GameManager.Instance.mapManager.Register(new Tile(TileType.King, gameObject), pos);
        self.SetPositionOnWorld();

        stats = Instantiate(card);
        _hp = stats.GetDEF();
        
        GetComponent<SpriteRenderer>().sprite = card.sprite;
        
        SetReady();
        Subscribers();
    }

    public override void Die()
    {
        base.Die();

        GameManager.Instance.eventManager.EndGameEvent(false);
    }
}
