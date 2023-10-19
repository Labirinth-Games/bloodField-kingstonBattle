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
        [SerializeField] private int moveUpHoverMouse = 30;

        private Vector3 _originalPosition;

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

            _originalPosition = Vector3.zero;
        }

        public void HoverEnter()
        {
            if (_originalPosition == Vector3.zero)
                _originalPosition = transform.position;

            var dir = _originalPosition;
            dir.y += moveUpHoverMouse;

            transform.DOMove(dir, .2f);
        }

        public void HoverExit()
        {
            var dir = _originalPosition;
            dir.y -= moveUpHoverMouse;

            transform.DOMove(_originalPosition, .2f);
        }
    }
}
