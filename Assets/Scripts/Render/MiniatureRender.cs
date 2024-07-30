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
            var kingSO = Resources.Load<CardSO>("Cards/King");
            var instance = Instantiate(prefab);
            instance.GetComponent<King>().OnCreate(kingSO, GameManager.Instance.UserId, 0, 0, isAttachment);

            return instance;
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
            if(prefab is null) return null;

            var instance = Instantiate(prefab);
            instance.GetComponent<MiniaturePreviewHUD>().Render(card, hp);

            instance.transform.SetParent(GameObject.FindGameObjectWithTag("HUD").transform);

            return instance;
        }

        public void SpawnRemote(string miniature, string card, int y, int x)
        {   
            var prefab = Resources.Load<GameObject>(miniature);
            var cardSO = Resources.Load<CardSO>(card);
            var instance = Instantiate(prefab);
            
            instance.GetComponent<Miniature>().OnCreate(cardSO, GameManager.Instance.UserId, y, x, false);
        }
    }
}
