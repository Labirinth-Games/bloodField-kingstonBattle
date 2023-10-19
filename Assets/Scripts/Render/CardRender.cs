using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Render
{
    public class CardRender : MonoBehaviour
    {
        public static (string _id, GameObject gameObject) Render(CardSO cardData)
        {
            GameObject prefab = GameManager.Instance.cardManager.cardPrefab;

            var instance = Instantiate(prefab);
            var parent = GameObject.FindGameObjectWithTag("Hand");

            instance.transform.SetParent(parent.transform);

            if (instance.TryGetComponent(out Card card))
            {
                card.Create(cardData);
            }

            return (card._id, instance);
        }

        public static GameObject PreviewRender(CardSO cardData)
        {
            GameObject prefab = GameManager.Instance.cardManager.cardPreviewPrefab;

            var instance = Instantiate(prefab);
            var parent = GameObject.FindGameObjectWithTag("HUD");

            instance.transform.SetParent(parent.transform);

            if (instance.TryGetComponent(out CardPreview card))
            {
                card.Create(cardData);
            }

            return instance;
        }
    }
}
