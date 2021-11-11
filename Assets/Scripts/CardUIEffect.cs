using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CardUIEffect  : MonoBehaviour , IDragHandler , IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityAction<int, Vector2> OnDragAction;
    public UnityAction<int> OnTapDownAction, OnTapReleaseAction, OnEnterAction, OnExitAction;
    public int cardId;
    public bool isDrag = true;

    public void OnDrag(PointerEventData eventData)
    {
        if(OnDragAction != null && isDrag)
            OnDragAction(cardId, eventData.delta);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(OnTapDownAction != null)
                OnTapDownAction(cardId);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(OnTapReleaseAction != null)
                OnTapReleaseAction(cardId);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(OnEnterAction != null)
                OnEnterAction(cardId);
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData eventData)
    {
        if(OnExitAction != null)
                OnExitAction(cardId);
    }
}
