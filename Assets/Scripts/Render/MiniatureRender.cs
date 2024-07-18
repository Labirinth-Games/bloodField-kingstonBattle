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
        public GameObject KingRender(GameObject prefab)
        {
            // SpawnServerRpc(new MiniatureCreateMessage()
            // {
            //     position = (0, 0),
            //     prefab = prefab
            // });
            var instance = Instantiate(prefab);

            return null; ///instance;
        }

        public GameObject Render(CardSO card, GameObject prefab)
        {
            // SpawnServerRpc(new MiniatureCreateMessage()
            // {
            //     position = (0, 0),
            //     card = card,
            //     prefab = prefab
            // });
            var instance = Instantiate(prefab);

            return null;
        }

        public GameObject PreviewRender(CardSO card, int hp, GameObject prefab)
        {
            var instance = Instantiate(prefab);
            instance.GetComponent<MiniaturePreviewHUD>().Render(card, hp);

            instance.transform.SetParent(GameObject.FindGameObjectWithTag("HUD").transform);

            return instance;
        }

        public void SpawnRemote(GameObject instance, string miniature)
        {
            instance.GetComponent<Miniature>().OnCreate(miniature);
        }
    }
}
