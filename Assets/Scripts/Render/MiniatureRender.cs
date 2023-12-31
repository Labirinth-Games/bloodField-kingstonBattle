using HUD;
using Managers;
using System.Collections;
using System.Collections.Generic;
using Miniatures;
using UnityEngine;

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

            var instance = Instantiate(prefab, GameObject.Find("Miniatures").transform);
            instance.GetComponent<Miniature>().Create(pos, card);
            instance.GetComponent<SpriteRenderer>().sprite = card.sprite;

            return instance;
        }

        public static GameObject PreviewRender(CardSO card, int hp, GameObject prefab)
        {
            var instance = Instantiate(prefab);
            instance.GetComponent<MiniaturePreviewHUD>().Render(card, hp);

            instance.transform.SetParent(GameObject.FindGameObjectWithTag("HUD").transform);

            return instance;
        }
    }
}
