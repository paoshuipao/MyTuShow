using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderEvent : MonoBehaviour , IInitializePotentialDragHandler, IDragHandler, IEndDragHandler
{

    public Action E_OnClick;
    public Action E_OnDrag;
    public Action E_OnDragEnd;



    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (null!= E_OnClick)
        {
            E_OnClick();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (null != E_OnDrag)
        {
            E_OnDrag();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (null != E_OnDragEnd)
        {
            E_OnDragEnd();
        }
    }




}
