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
        private GameObject _instance;
        private GameObject _prefab;

        public GameObject KingRender(GameObject prefab)
        {
            _prefab = prefab;

            SpawnServerRpc(new MiniatureCreateMessage()
            {
                position = (0, 0)
            });

            return null; ///instance;
        }

        public GameObject Render(CardSO card, GameObject prefab)
        {
            _prefab = prefab;

            SpawnServerRpc(new MiniatureCreateMessage()
            {
                position = (0, 0),
                card = card
            });

            return null;
        }

        public GameObject PreviewRender(CardSO card, int hp, GameObject prefab)
        {
            var instance = Instantiate(prefab);
            instance.GetComponent<MiniaturePreviewHUD>().Render(card, hp);

            instance.transform.SetParent(GameObject.FindGameObjectWithTag("HUD").transform);

            return instance;
        }

        [Command(requiresAuthority = false)]
        public void SpawnServerRpc(MiniatureCreateMessage miniature, NetworkConnectionToClient sender = null)
        {
            _instance = Instantiate(_prefab);
            NetworkServer.Spawn(_instance, sender.identity.connectionToClient);

            NetworkServer.SendToAll(new MiniatureCreateMessage()
            {
                position = miniature.position,
                card = miniature.card
            });
        }
    }
}
