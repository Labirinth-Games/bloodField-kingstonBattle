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

        public void Render(CardSO cardStats)
        {
            Title.text = cardStats.title;
            ATKField.text =   $"ATK  -------   {cardStats.ATK}";
            DEFField.text =   $"DEF  -------   {cardStats.DEF}";
            MOVField.text =   $"MOV  -------   {cardStats.MOV}";
            D_ATKField.text = $"D_ATK  ----   {cardStats.D_ATK}";
            image.sprite = cardStats.sprite;
        }

    }
}
