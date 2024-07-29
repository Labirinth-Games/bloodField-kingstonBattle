using BloodField.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class TurnHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TextMeshProUGUI DisplayCountTurn;
        [SerializeField] private Button button;

        private int turnNumber = 2;

        private void FixedUpdate()
        {
            if (label != null)
            {
                if (!GameManager.Instance.matchManager.IsReadyPreparationPhasePlayer())
                {
                    label.text = "Preparation Stage";
                    button.GetComponentInChildren<TextMeshProUGUI>().text = "Ready!";
                    return;
                }

                if (GameManager.Instance.matchManager.IsReadyPreparationPhasePlayer() && !GameManager.Instance.matchManager.IsMainPhase())
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

        void Start()
        {
            GameManager.Instance.eventManager.OnStartMainPhase += () =>
            {
                DisplayCountTurn.text = "Turn 1";
            };
            GameManager.Instance.eventManager.OnStartMyTurn += () => DisplayCountTurn.text = $"Turn {turnNumber++}";
            GameManager.Instance.eventManager.OnEndGame += (bool isWin) => gameObject.SetActive(false);
        }
    }

}