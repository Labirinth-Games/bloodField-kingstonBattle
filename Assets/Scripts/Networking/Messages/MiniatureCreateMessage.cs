using Mirror;
using UnityEngine;

public struct MiniatureCreateMessage : NetworkMessage
{
    public (int y, int x) position;
    public CardSO card;
    public GameObject prefab;
}

public static class CustomReadWriteMiniatureCreateSerializer
{
    public static void WriteMyType(this NetworkWriter writer, MiniatureCreateMessage miniature)
    {
        writer.WriteInt(miniature.position.y);
        writer.WriteInt(miniature.position.x);

        if(miniature.prefab is not null)
            writer.WriteString($"Miniatures/{miniature.prefab.name}");
        else
            writer.WriteString(null);

        if (miniature.card is not null)
            writer.WriteString($"Cards/{miniature.card.type}/{miniature.card.name}");
        else
            writer.WriteString(null);
    }

    public static MiniatureCreateMessage ReadMyType(this NetworkReader reader)
    {
        (int y, int x) position = (reader.ReadInt(), reader.ReadInt());
        string prefabPath = reader?.ReadString();
        string cardPath = reader?.ReadString();
        CardSO card = null;
        GameObject prefab = null;

        if (prefabPath != null)
            prefab = Resources.Load<GameObject>(prefabPath);

        if (cardPath != null)
            card = Resources.Load<CardSO>(cardPath);

        return new MiniatureCreateMessage()
        {
            position = position,
            card = card,
            prefab = prefab
        };
    }
}