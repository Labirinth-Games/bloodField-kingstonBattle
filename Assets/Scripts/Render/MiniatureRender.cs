using HUD;
using BloodField.Managers;
using System.Collections;
using System.Collections.Generic;
using Miniatures;
using UnityEngine;

namespace Render
{
    public class MiniatureRender : MonoBehaviour
    {
        public static GameObject KingRender(GameObject prefab)
        {
            var kingSO = Resources.Load<CardSO>("Cards/King");
            var instance = Instantiate(prefab);
            instance.GetComponent<King>().OnCreate(kingSO, GameManager.Instance.UserId, 0, 0);

            return instance;
        }

        public static GameObject Render(CardSO card, GameObject prefab)
        {
            var instance = Instantiate(prefab);
            instance.GetComponent<Miniature>().OnCreate(card, GameManager.Instance.UserId, 0, 0);

            return instance;
        }

        public static GameObject PreviewRender(CardSO card, int hp, GameObject prefab)
        {
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
            
            instance.GetComponent<Miniature>().OnCreate(cardSO, GameManager.Instance.UserId, y, x);
        }
    }
}
