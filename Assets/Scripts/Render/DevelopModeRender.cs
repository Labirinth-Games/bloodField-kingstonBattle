using BloodField.Managers;
using UnityEngine;

namespace Render
{
    class DevelopModeRender : MonoBehaviour
    {
        void Start()
        {
            gameObject.SetActive(GameManager.Instance.isDevelopMode);
        }
    }
}