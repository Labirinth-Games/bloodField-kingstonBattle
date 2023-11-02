using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Network
{
    public class LobbyPlayer : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI StatusPrefab;

        [SyncVar(hook = nameof(HandlerNameUpdate))] public string displayName;
        [SyncVar] public int connectionId;


        public override void OnStartAuthority()
        {
            CmdSetPlayerSettings(displayName);
        }

        public override void OnStartClient()
        {
            transform.SetParent(GameObject.Find("Players Name").gameObject.transform);

            GetComponentInChildren<TextMeshProUGUI>().text = displayName;
        }

        #region Commands
        [Command]
        private void CmdSetPlayerSettings(string name)
        {
            displayName = name;
        }

        #endregion

        #region Handlers
        public void HandlerNameUpdate(string oldValue, string newValue)
        {
            gameObject.name = displayName;
            GetComponentInChildren<TextMeshProUGUI>().text = newValue;
        }

        #endregion
    }

}