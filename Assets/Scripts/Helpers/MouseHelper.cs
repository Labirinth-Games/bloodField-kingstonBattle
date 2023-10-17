using Managers;
using Render;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseHelper : MonoBehaviour
{
    public UnityEvent<Card> OnCardSelected;
    public UnityEvent<Card> OnCardHoverEnter;
    public UnityEvent<Card> OnCardHoverExit;

    [SerializeField] private Card _isMouseHover;

    private void Update()
    {
        SelectCard();
        PreviewCard();

        if(_isMouseHover != GetCard())
        {
            OnCardHoverExit?.Invoke(_isMouseHover);
            _isMouseHover = GetCard();
            OnCardHoverEnter?.Invoke(_isMouseHover);
        }
    }

    public void SelectCard()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameManager.Instance.cardManager.ClosePreview();

            Card card = GetCard();

            if(card != null)
               OnCardSelected?.Invoke(card);
        }
    } 
    
    public void PreviewCard()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Card card = GetCard();

            if(card != null)
                GameManager.Instance.cardManager.Preview(card);
        }
    }


    private Card FindCard(GameObject element)
    {
        if(element.TryGetComponent(out Card card))
        {
            return card;
        }

        return FindCard(element.transform.parent.gameObject);
    }

    private Card GetCard()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (raycastResults.Count > 0)
        {
            return FindCard(raycastResults[0].gameObject);
        }

        return null;
    }
}
