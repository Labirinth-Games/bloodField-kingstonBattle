using System.Collections.Generic;
using Mirror;
using UnityEngine;


public struct CardDrawSerializerNetwork
{
    public List<CardSO> cards;

    public CardDrawSerializerNetwork(List<CardSO> cards)
    {
        this.cards = cards;
    }
}

public static class CustomReadWriteCardListSerializer
{
    public static void WriteMyType(this NetworkWriter writer, CardDrawSerializerNetwork cardDraw)
    {
        List<string> cardNames = new List<string>();

        foreach (CardSO item in cardDraw.cards)
            cardNames.Add($"Cards/{item.type}/{item.name}");

        writer.WriteList<string>(cardNames);
    }

    public static CardDrawSerializerNetwork ReadMyType(this NetworkReader reader)
    {
        List<CardSO> cards = new List<CardSO>();

        foreach (string name in reader.ReadList<string>())
            cards.Add(Resources.Load<CardSO>(name));

        return new CardDrawSerializerNetwork(cards);
    }
}