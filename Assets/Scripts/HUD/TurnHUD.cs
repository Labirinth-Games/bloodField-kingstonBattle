using Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HUD
{
    public class TurnHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        private void FixedUpdate()
        {
            if(label != null)
            {
                label.text = GameManager.Instance.turnManager.IsMyTurn() ? "My Turn" : "Wait...";
            }
        }
    }

}