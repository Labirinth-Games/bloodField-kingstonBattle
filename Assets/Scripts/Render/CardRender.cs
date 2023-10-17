using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Render
{
    public class CardRender : MonoBehaviour
    {
        public static (string _id, GameObject gameObject) Render(CardSO cardData, bool isPreview = false)
        {
            GameObject prefab = isPreview ? GameManager.Instance.cardManager.cardPreviewPrefab : GameManager.Instance.cardManager.cardPrefab;

            var instance = Instantiate(prefab);
            var parent = GameObject.FindGameObjectWithTag(isPreview ? "HUD" : "Hand");

            instance.transform.SetParent(parent.transform);

            if (instance.TryGetComponent(out Card card))
            {
                card.Create(cardData);
            }

            return (card._id, instance);
        }
    }
}
