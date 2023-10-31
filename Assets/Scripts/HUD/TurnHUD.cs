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
            if (label != null)
            {
                if (GameManager.Instance.turnManager.IsTurnPreparation())
                {
                    label.text = "Preparation stage";
                    return;
                }

                label.text = GameManager.Instance.turnManager.IsMyTurn() ? "You Turn" : "Wait...";
            }
        }
    }

}