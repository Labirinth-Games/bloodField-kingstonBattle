using System.Collections.Generic;
using BloodField.Managers;
using TMPro;
using UnityEngine;

namespace BloodField.Network
{
    public class MatchHUD : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI DisplayHud;
        [SerializeField] private GameObject playerContainer;
        [SerializeField] private GameObject playerLobbyPrefab;
        [SerializeField] private GameObject ContainerListPlayerLobbyPrefab;

        public void SetMessageConnection(string text)
        {
            DisplayHud.text = text;
        }

        public void SetPlayerList(List<string> playerNames)
        {
            ContainerListPlayerLobbyPrefab.SetActive(true);
            
            foreach (var names in playerNames)
            {
                var instance = Instantiate(playerLobbyPrefab);
                instance.GetComponent<TextMeshProUGUI>().text = names;
                instance.transform.SetParent(playerContainer.transform);
            }
        }

        void Start()
        {
            GameManager.Instance.eventManager.OnDisplayMessageFindMatchHUD += SetMessageConnection;
            GameManager.Instance.eventManager.OnDisplayPlayersOnLobbyHUD += SetPlayerList;
        }
    }
}