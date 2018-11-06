using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UGUI_PointEnterAndExit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Action E_OnMouseEnter;
    public Action E_OnMouseExit;

    public GameObject EnterSee;         // 进入要显示的对象


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (null!= E_OnMouseEnter)
        {
            E_OnMouseEnter();
        }
        if (null!= EnterSee)
        {
            EnterSee.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (null != E_OnMouseExit)
        {
            E_OnMouseExit();
        }

        if (null != EnterSee)
        {
            EnterSee.SetActive(false);
        }
    }






}

