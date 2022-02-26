using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CardUIEffect  : MonoBehaviour , IDragHandler , IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardData thisCardData;
    public UnityAction<int, Vector2> OnDragAction;
    public UnityAction<int> OnTapDownAction, OnTapReleaseAction, OnEnterAction, OnExitAction;
    public int cardId;
    public bool isDrag = true;

    /// <summary>
    /// 是否可以使用卡牌
    /// </summary>
    /// <param name="_enable">是否</param>
    public void ActivateCard(bool _enable){
        if(_enable){
            isDrag = true;
            GetComponent<Animator>().SetBool("Enable", true);
        }else{
            isDrag = false;
            GetComponent<Animator>().SetBool("Enable", false);
        }
    }

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
    public void OnPointerExit(PointerEventData eventData)
    {
        if(OnExitAction != null)
                OnExitAction(cardId);
    }
}
