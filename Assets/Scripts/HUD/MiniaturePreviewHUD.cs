using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class MiniaturePreviewHUD : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI Title;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI ATKField;
        [SerializeField] private TextMeshProUGUI DEFField;
        [SerializeField] private TextMeshProUGUI MOVField;
        [SerializeField] private TextMeshProUGUI D_ATKField;

        public void Render(CardSO cardStats, int hp)
        {
            Title.text = cardStats.title;
            ATKField.text =   $"ATK   ---- {cardStats.GetATK()}";
            DEFField.text =   $"DEF   ---- {hp}";
            MOVField.text =   $"MOV   ---- {cardStats.GetMOV()}";
            D_ATKField.text = $"D_ATK ---- {cardStats.GetD_ATK()}";
            image.sprite = cardStats.sprite;
        }

    }
}
