using Mirror;
using UnityEngine;


public class GamePlayer : NetworkBehaviour
{
    [Header("Settings")]
    public string displayName;
    public Color playerColor;
    public int connectionId;

    [Header("Debug")]
    public bool hasPCTest = false;

    private bool _isFacingRight = true;

    [SyncVar]
    private Vector3 _dir;

    #region Utils
    public void Flip()
    {
        Vector3 currentScale = GetComponentInChildren<Renderer>().transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        _isFacingRight = !_isFacingRight;
    }
    #endregion

    #region Unity Events
    public override void OnStartLocalPlayer()
    {

    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    #endregion
}

