using Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class TurnHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Button button;

        private void FixedUpdate()
        {
            if (label != null)
            {
                if (GameManager.Instance.turnManager.IsTurnPreparation())
                {
                    label.text = "Preparation Stage";
                    button.GetComponentInChildren<TextMeshProUGUI>().text = "Ready!";
                    return;
                }

                if (!GameManager.Instance.turnManager.IsAllReadyToInitGame())
                {
                    label.text = "Wait players is ready";
                    button.GetComponentInChildren<TextMeshProUGUI>().text = "End";
                    return;
                }

                label.text = GameManager.Instance.turnManager.IsMyTurn() ? "You Turn" : "Wait...";
            }
        }

        private void Start() {
            button.onClick.AddListener(GameManager.Instance.turnManager.EndTurnButtonAction);
        }
    }

}