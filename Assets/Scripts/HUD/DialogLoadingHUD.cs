using BloodField.Managers;
using TMPro;
using UnityEngine;

namespace HUD
{
    public class DialogLoadingHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private GameObject screenPrefab;

        public void ShowScreen(string message)
        {
            GameManager.Instance.screenManager.Clear();
            
            label.text = message;
            screenPrefab.SetActive(true);
        }

        public void HideScreen()
        {
            screenPrefab.SetActive(false);
        }

        void Start()
        {
            GameManager.Instance.eventManager.OnShowScreenLoadHUD += ShowScreen;
            GameManager.Instance.eventManager.OnHideScreenLoadHUD += HideScreen;
        }

        void OnDestroy()
        {
            GameManager.Instance.eventManager.OnShowScreenLoadHUD -= ShowScreen;
            GameManager.Instance.eventManager.OnHideScreenLoadHUD -= HideScreen;
        }
    }
}