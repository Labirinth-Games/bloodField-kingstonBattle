using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Render;

public class Player : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject kingPrefab;

    [Header("Settings")]
    public string displayName;
    public int connectionId;

    [SerializeField] private List<CardSO> hand = new List<CardSO>();

    public void SetCardOnHand(List<CardSO> cards) => hand.AddRange(cards);

    #region Events

    public void OnStartPlayerToWorld()
    {
        Debug.Log($"calling the player set {netId} Im {isLocalPlayer}");

        GameManager.Instance.miniatureRender.KingRender(kingPrefab);
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        GameManager.Instance.player = this;
    }

    #endregion

    #region Unity Events
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    #endregion
}
