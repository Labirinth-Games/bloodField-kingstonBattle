using HUD;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miniatures;

namespace Render
{
    public class MiniatureRender : MonoBehaviour
    {
        public static GameObject KingRender((int y, int x) position, GameObject prefab)
        {
            var instance = Instantiate(prefab);
            instance.GetComponent<Miniature>().Create(position);

            return instance;
        }

        public static GameObject Render(CardSO card, GameObject prefab)
        {
            (int y, int x) pos = (0, 0);

            var instance = Instantiate(prefab);
            instance.GetComponent<Miniature>().Create(pos, card);
            instance.GetComponent<SpriteRenderer>().sprite = card.sprite;

            instance.transform.SetParent(GameObject.Find("Miniatures").transform);

            return instance;
        }

        public static GameObject PreviewRender(CardSO card, GameObject prefab)
        {
            var instance = Instantiate(prefab);
            instance.GetComponent<MiniaturePreviewHUD>().Render(card);

            instance.transform.SetParent(GameObject.FindGameObjectWithTag("HUD").transform);

            return instance;
        }
    }
}
