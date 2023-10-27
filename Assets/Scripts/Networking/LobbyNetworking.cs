using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyNetworking : NetworkBehaviour
{
    [SerializeField] private NetworkHud hud;

    private NetworkVariable<int> connAmount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    private NetworkVariable<bool> hasHostStart = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone);


    private void Awake()
    {
        hud.hostButton?.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            hasHostStart.Value = true;
            hud.IsHosting = true;
        });
        hud.clientButton?.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            hud.IsHosting = true;
        });

        hud.startButton?.onClick.AddListener(() =>
        {
            // hasHostStart.Value = true;
            if (IsHost)
                StartGameClientRpc();
        });
    }

    [ClientRpc]
    public void StartGameClientRpc()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void Update()
    {
        hud.connLabel.text = $"Conn {connAmount.Value}";

        if (IsHost && hud.IsHosting)
            hud.ShowStart();
        else if (IsClient && hud.IsHosting)
            hud.WaitHost();

        if (IsServer)
            connAmount.Value = NetworkManager.Singleton.ConnectedClients.Count;

    }
}
