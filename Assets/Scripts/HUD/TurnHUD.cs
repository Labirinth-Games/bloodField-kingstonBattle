using BloodField.Managers;
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
                if (!GameManager.Instance.matchManager.IsPlayerLocalReady())
                {
                    label.text = "Preparation Stage";
                    button.GetComponentInChildren<TextMeshProUGUI>().text = "Ready!";
                    return;
                }

                if (GameManager.Instance.matchManager.IsPlayerLocalReady() && !GameManager.Instance.matchManager.IsMainPhase())
                {
                    label.text = "Wait players is ready";
                    button.GetComponentInChildren<TextMeshProUGUI>().text = "End";
                    return;
                }

                if (GameManager.Instance.matchManager.IsMainPhase())
                {
                    label.text = GameManager.Instance.turnManager.IsMyTurn() ? "You Turn" : "Wait...";
                    return;
                }

                label.text = "Loading...";
            }
        }

        public void TurnActionButton()
        {
            if (GameManager.Instance.matchManager.IsPreparationPhase()) GameManager.Instance.turnPreparationManager.EndTurnPreparation();
            if (GameManager.Instance.matchManager.IsMainPhase()) GameManager.Instance.turnManager.EndTurn();
        }
    }

}