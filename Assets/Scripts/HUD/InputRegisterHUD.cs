using System;
using BloodField.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HUD
{
    public class InputRegisterHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        private string _displayName;

        public void SetDisplayName(string name) => _displayName = name;

        public async void Register()
        {
            if (string.IsNullOrEmpty(_displayName))
            {
                label.text = "You need to choose a name";
                return;
            }

            try
            {
                await GameManager.Instance.networkManager.UpdateUserRegister(_displayName);
            }
            catch (ApplicationException err)
            {
                label.text = err.Message;
                return;
            }

            SceneManager.LoadScene("GameScene");
        }
    }
}