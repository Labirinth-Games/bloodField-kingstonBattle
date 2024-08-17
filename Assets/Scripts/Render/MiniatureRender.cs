using HUD;
using BloodField.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BloodField.Miniatures;

namespace Render
{
    public class MiniatureRender : MonoBehaviour
    {
        public static GameObject KingRender(GameObject prefab, bool isAttachment = true)
        {
            var kingSO = Resources.Load<CardSO>("Cards/King/King");
            var king = Instantiate(prefab).GetComponent<King>();
            king.OnCreate(kingSO, GameManager.Instance.UserId, 0, 0, isAttachment);

            GameManager.Instance.eventManager.MiniatureCreatedEvent(king._id, king.stats, king.self);

            return king.gameObject;
        }

        public static GameObject Render(CardSO card, GameObject prefab, bool isAttachment = true)
        {
            var instance = Instantiate(prefab);
            instance.GetComponent<Miniature>().OnCreate(card, GameManager.Instance.UserId, 0, 0, isAttachment);

            return instance;
        }

        public static GameObject Render(CardSO card, GameObject prefab, (int y, int x) position, bool isAttachment = true)
        {
            var instance = Instantiate(prefab);
            instance.GetComponent<Miniature>().OnCreate(card, GameManager.Instance.UserId, position.y, position.x, isAttachment);

            return instance;
        }

        public static GameObject PreviewRender(CardSO card, int hp, GameObject prefab)
        {
            if (prefab is null) return null;

            var instance = Instantiate(prefab);
            instance.GetComponent<MiniaturePreviewHUD>().Render(card, hp);

            instance.transform.SetParent(GameObject.FindGameObjectWithTag("HUD").transform);

            return instance;
        }

        public static GameObject SpawnRemote(string id, CardSO card, (int y, int x) pos)
        {
            var instance = new GameObject();
            instance.name = $"Remote_{card.type}_{card.title}";
            instance.AddComponent<MiniatureRemote>();
            instance.AddComponent<SpriteRenderer>().sortingLayerName = "Miniature";
            instance.GetComponent<SpriteRenderer>().sortingOrder = -1;
            instance.AddComponent<BoxCollider2D>();
            instance.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
            instance.AddComponent<SignageUI>();

            var reflexPosition = GameManager.Instance.mapManager.ReflexPosition(pos);

            instance.GetComponent<MiniatureRemote>().OnCreate(id, card, GameManager.Instance.UserId, reflexPosition.y, reflexPosition.x);

            return instance;
        }
    }
}
