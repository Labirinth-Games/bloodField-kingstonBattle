using Mirror;
using UnityEngine;

public struct MiniatureCreateMessage : NetworkMessage
{
    public (int y, int x) position;
    public CardSO card;
}

public static class CustomReadWriteMiniatureCreateSerializer
{
    public static void WriteMyType(this NetworkWriter writer, MiniatureCreateMessage miniature)
    {
        writer.WriteInt(miniature.position.y);
        writer.WriteInt(miniature.position.x);

        if (miniature.card is not null)
            writer.WriteString($"Cards/{miniature.card.type}/{miniature.card.name}");
        else
            writer.WriteString(null);
    }

    public static MiniatureCreateMessage ReadMyType(this NetworkReader reader)
    {
        var position = (reader.ReadInt(), reader.ReadInt());
        var cardName = reader?.ReadString();
        CardSO card = null;

        if (cardName != null)
            card = Resources.Load<CardSO>(cardName);

        return new MiniatureCreateMessage()
        {
            position = position,
            card = card
        };
    }
}