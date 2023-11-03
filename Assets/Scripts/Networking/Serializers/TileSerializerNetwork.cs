using Mirror;
using UnityEngine;

public struct TileSerializerNetwork
{
    public (int y, int x) position
    {
        get { return (y, x); }
    }
    public int y;
    public int x;
    public GameObject prefab;

    public TileSerializerNetwork(int y = 0, int x = 0, GameObject prefab = null)
    {
        this.y = y;
        this.x = x;
        this.prefab = prefab;
    }

    public TileSerializerNetwork((int y, int x) position, GameObject prefab = null)
    {
        this.y = position.y;
        this.x = position.x;
        this.prefab = prefab;
    }
}

public static class CustomReadWriteTileSerializer
{
    public static void WriteMyType(this NetworkWriter writer, TileSerializerNetwork tile)
    {
        writer.WriteInt(tile.y);
        writer.WriteInt(tile.x);
    }

    public static TileSerializerNetwork ReadMyType(this NetworkReader reader)
    {
        return new TileSerializerNetwork(reader.ReadInt(), reader.ReadInt());
    }
}