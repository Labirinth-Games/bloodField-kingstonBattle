using Mirror;
using UnityEngine;

public static class CustomReadWriteCardSerializer
{
    public static void WriteMyType(this NetworkWriter writer, CardSO card)
    {
        writer.WriteString(card.name);
    }

    public static CardSO ReadMyType(this NetworkReader reader)
    {
        return (CardSO)Resources.Load(reader.ReadString());
    }
}