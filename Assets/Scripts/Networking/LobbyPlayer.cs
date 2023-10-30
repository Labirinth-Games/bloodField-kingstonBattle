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

        // [SyncVar(hook = nameof(HandlerColorUpdate))] public Color playerColor;
        [SyncVar(hook = nameof(HandlerNameUpdate))] public string displayName;
        // [SyncVar(hook = nameof(HandlerStatusUpdate))] private bool _isReady = false;
        [SyncVar] public int connectionId;


        public override void OnStartAuthority()
        {
            // playerColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            // playerName = PlayerPrefs.GetString("PlayerName");

            // gameObject.name = playerName;

            CmdSetPlayerSettings(displayName);
        }

        public override void OnStartClient()
        {
            transform.SetParent(GameObject.Find("Players Name").gameObject.transform);

            //StatusReadyUI(_isReady);

            GetComponentInChildren<TextMeshProUGUI>().text = displayName;
            // GetComponentInChildren<UnityEngine.UI.Image>().color = playerColor;
        }

        // public bool GetIsReady()
        // {
        //     return _isReady;
        // }

        // private void StatusReadyUI(bool value)
        // {
        //     if (value)
        //     {
        //         StatusPrefab.text = "Ready";
        //         StatusPrefab.color = Color.green;
        //     }
        //     else
        //     {
        //         StatusPrefab.text = "No Ready";
        //         StatusPrefab.color = Color.red;
        //     }
        // }

        #region Commands
        [Command]
        private void CmdSetPlayerSettings(string name)
        {
            // this.playerColor = color;
            displayName = name;
        }

        // [Command]
        // public void CmdSetReadyToggle()
        // {
        //     _isReady = !_isReady;
        // }

        #endregion

        #region Handlers
        public void HandlerNameUpdate(string oldValue, string newValue)
        {
            gameObject.name = displayName;
            GetComponentInChildren<TextMeshProUGUI>().text = newValue;
        }

        // public void HandlerColorUpdate(Color oldColor, Color newColor)
        // {
        //     GetComponentInChildren<UnityEngine.UI.Image>().color = newColor;
        // }

        // public void HandlerStatusUpdate(bool oldStatus, bool newStatus)
        // {
        //     StatusReadyUI(newStatus);
        // }
        #endregion
    }

}