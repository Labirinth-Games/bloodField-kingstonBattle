using System.Collections.Generic;
using Mirror;

public struct SignagePositionsSerializerNetowrk : NetworkMessage
{
    public int x;
    public int y;

    public (int y, int x) position
    {
        get { return (y, x); }
    }
}

public struct SignageUISerializerNetowrk : NetworkMessage
{
    public bool isAttack;
    public List<SignagePositionsSerializerNetowrk> positions;
}

public static class CustomReadWriteSignageUISerializer
{
    public static void WriteMyType(this NetworkWriter writer, SignageUISerializerNetowrk signage)
    {
        writer.WriteList(signage.positions);
        writer.WriteBool(signage.isAttack);
    }

    public static SignageUISerializerNetowrk ReadMyType(this NetworkReader reader)
    {
        return new SignageUISerializerNetowrk()
        {
            positions = reader.ReadList<SignagePositionsSerializerNetowrk>(),
            isAttack = reader.ReadBool(),
        };
    }
}