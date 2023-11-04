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
        public GameObject KingRender(GameObject prefab)
        {
            SpawnServerRpc(new MiniatureCreateMessage()
            {
                position = (0, 0),
                prefab = prefab
            });

            return null; ///instance;
        }

        public GameObject Render(CardSO card, GameObject prefab)
        {
            SpawnServerRpc(new MiniatureCreateMessage()
            {
                position = (0, 0),
                card = card,
                prefab = prefab
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
            var instance = Instantiate(miniature.prefab);
            NetworkServer.Spawn(instance, sender.identity.connectionToClient);

            SpawnClientRpc(instance, miniature);
        }

        [ClientRpc]
        public void SpawnClientRpc(GameObject instance, MiniatureCreateMessage miniature)
        {
            instance.GetComponent<Miniature>().OnCreate(miniature);
        }
    }
}
