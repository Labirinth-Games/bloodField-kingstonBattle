using HUD;
using Managers;
using System.Collections;
using System.Collections.Generic;
using Miniatures;
using UnityEngine;
using Mirror;

namespace Render
{
    public class MiniatureRender : NetworkBehaviour
    {
        private GameObject _prefab;
        private GameObject _instance;

        public GameObject KingRender(GameObject prefab)
        {
            _prefab = prefab;

            SpawnServerRpc();
            return null; ///instance;
        }

        [Command(requiresAuthority = false)]
        public void SpawnServerRpc(NetworkConnectionToClient sender = null)
        {
            _instance = Instantiate(_prefab);
            NetworkServer.Spawn(_instance, sender.identity.connectionToClient);
        }

        public GameObject Render(CardSO card, GameObject prefab)
        {
            (int y, int x) pos = (0, 0);

            var instance = Instantiate(prefab, GameObject.Find("Miniatures").transform);
            instance.GetComponent<Miniature>().Create(pos, card);
            instance.GetComponent<SpriteRenderer>().sprite = card.sprite;

            return instance;
        }

        public GameObject PreviewRender(CardSO card, int hp, GameObject prefab)
        {
            var instance = Instantiate(prefab);
            instance.GetComponent<MiniaturePreviewHUD>().Render(card, hp);

            instance.transform.SetParent(GameObject.FindGameObjectWithTag("HUD").transform);

            return instance;
        }
    }
}
