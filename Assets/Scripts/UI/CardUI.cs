using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace UI
{
    public class CardUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI Title;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI ATKField;
        [SerializeField] private TextMeshProUGUI DEFField;
        [SerializeField] private TextMeshProUGUI MOVField;
        [SerializeField] private TextMeshProUGUI D_ATKField;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI type;
        [SerializeField] private GameObject badgeGroup;
        [SerializeField] private GameObject icons;
        [SerializeField] private int moveUpHoverMouse = 30;

        public void Render(CardSO cardStats)
        {
            Title.text = cardStats.title;
            ATKField.text = cardStats.ATK.ToString();
            DEFField.text = cardStats.DEF.ToString();
            MOVField.text = cardStats.MOV.ToString();
            D_ATKField.text = cardStats.D_ATK.ToString();
            description.text = cardStats.description;
            type.text = cardStats.type.ToString();
            image.sprite = cardStats.sprite;

            badgeGroup.SetActive(cardStats.isGroup);

            transform.DOScale(0, .3f).From();

            if (cardStats.type == Cards.CardTypeEnum.Command)
                icons.SetActive(false);
        }

        public void HoverEnter() => transform.DOLocalMoveY(moveUpHoverMouse, .1f);

        public void HoverExit() => transform.DOLocalMoveY(0, .1f);

        public void Click() => transform.DOScale(0, .1f).OnComplete(() => Destroy(gameObject));
    }
}
