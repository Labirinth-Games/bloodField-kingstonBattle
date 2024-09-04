using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace HUD
{
    public class DialogHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        public async void SetText(string text, int delayToShowWord = 100)
        {
            var textSplit = text.Split(" ");
            
            foreach (var word in textSplit)
            {
                label.text += " " + word;
                await Task.Delay(delayToShowWord);
            }
        }

        public void Clear() => label.text = "";
    }
}