using Mirror;
using UnityEngine;

public struct TileSerializeNetwork
{
    public (int y, int x) position
    {
        get { return (y, x); }
    }
    public int y;
    public int x;
    public GameObject prefab;

    public TileSerializeNetwork(int y = 0, int x = 0, GameObject prefab = null)
    {
        this.y = y;
        this.x = x;
        this.prefab = prefab;
    }

    public TileSerializeNetwork((int y, int x) position, GameObject prefab = null)
    {
        this.y = position.y;
        this.x = position.x;
        this.prefab = prefab;
    }
}

public static class CustomReadWriteFunctions
{
    public static void WriteMyType(this NetworkWriter writer, TileSerializeNetwork value)
    {
        writer.WriteInt(value.y);
        writer.WriteInt(value.x);
    }

    public static TileSerializeNetwork ReadMyType(this NetworkReader reader)
    {
        return new TileSerializeNetwork(reader.ReadInt(), reader.ReadInt());
    }
}