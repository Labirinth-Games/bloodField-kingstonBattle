using BloodField.Managers;
using TMPro;
using UnityEngine;

namespace BloodField.HUD
{
    public class LogHUD : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private GameObject elementItem;

        public void AddMessage(string message)
        {
            var instance = Instantiate(elementItem);
            instance.GetComponent<TextMeshProUGUI>().text = message;

            instance.transform.SetParent(container.transform);

            Destroy(instance, GameManager.Instance.GameSettings.LogMessageDurationTime);
        }
    }
}