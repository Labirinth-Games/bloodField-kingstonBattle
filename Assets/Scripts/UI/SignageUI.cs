using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Mirror;
using Tiles;
using UnityEngine;

public class SignageUI : NetworkBehaviour
{
    [SerializeField] private Sprite moveOverlayerSprite;
    [SerializeField] private Sprite reachOverlayerSprite;

    private List<GameObject> _instances = new List<GameObject>();

    public void OverlayAttack(List<Tile> tiles) {
        var positions = new List<SignagePositionsSerializerNetowrk>();
        tiles.ForEach(f => positions.Add(new SignagePositionsSerializerNetowrk() { x = f.position.x, y = f.position.y }));

        MarkUIServerRpc(new SignageUISerializerNetowrk() { isAttack = true, positions = positions });
        MarkUI(tiles, reachOverlayerSprite, true);
    }

    public void OverlayAttack(List<(int y, int x)> positions)
    {
        List<Tile> tiles = new List<Tile>();
        positions.ToList().ForEach(f => tiles.Add(new Tile(f.y, f.x)));

        var positionsServer = new List<SignagePositionsSerializerNetowrk>();
        positions.ForEach(f => positionsServer.Add(new SignagePositionsSerializerNetowrk() { x = f.x, y = f.y }));

        MarkUIServerRpc(new SignageUISerializerNetowrk() { isAttack = true, positions = positionsServer });
        MarkUI(tiles, reachOverlayerSprite, true);
    }

    public void OverlayMove(List<Tile> tiles)
    {
        var positions = new List<SignagePositionsSerializerNetowrk>();
        tiles.ForEach(f => positions.Add(new SignagePositionsSerializerNetowrk() { x = f.position.x, y = f.position.y }));

        MarkUIServerRpc(new SignageUISerializerNetowrk() { isAttack = false, positions = positions });
        MarkUI(tiles, moveOverlayerSprite);
    }

    #region Network Create Marker
    [Command(requiresAuthority = false)]
    public void MarkUIServerRpc(SignageUISerializerNetowrk signage)
    {
        OnSignageReceivedClientRpc(signage);
    }

    [ClientRpc(includeOwner = false)]
    public void OnSignageReceivedClientRpc(SignageUISerializerNetowrk signage)
    {
        Debug.Log($"Cliente recebeu a mensagem {netId}");

        List<Tile> tiles = new List<Tile>();
        signage.positions.ForEach(f => tiles.Add(new Tile(GameManager.Instance.mapManager.ReflexPosition(f.position))));

        MarkUI(tiles, !signage.isAttack ? moveOverlayerSprite : reachOverlayerSprite, signage.isAttack);
    }
    #endregion

    private void MarkUI(List<Tile> tiles, Sprite sprite, bool isOpacity = false)
    {
        tiles.ForEach(tile =>
        {
            var map = GameObject.Find("Map/OverlayActions");
            var instance = new GameObject();
            instance.AddComponent<SpriteRenderer>();

            instance.GetComponent<SpriteRenderer>().sprite = sprite;
            instance.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
            instance.transform.SetParent(map.transform);

            if (isOpacity)
            {
                instance.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .3f);
                instance.GetComponent<SpriteRenderer>().sortingOrder = -1;
            }

            instance.transform.position = tile.GetPositionOnWorld();

            _instances.Add(instance);
        });
    }

    public void Clear()
    {
        ClearMarkUIServerRpc();
        foreach (var instance in _instances)
        {
            Destroy(instance);
        }

        _instances.Clear();
    }

    [Command(requiresAuthority = false)]
    public void ClearMarkUIServerRpc()
    {
        ClearMarkUIClientRpc();
    }

    [ClientRpc(includeOwner = false)]
    public void ClearMarkUIClientRpc()
    {
        foreach (var instance in _instances)
        {
            Destroy(instance);
        }

        _instances.Clear();
    }
}
