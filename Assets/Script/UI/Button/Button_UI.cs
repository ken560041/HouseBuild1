using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class Button_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    // Start is called before the first frame update

    public Action ClickFunc = null;

    private float mouseOverPerSecFuncTimer;
    private bool mouseOver;

    //Color
    private Action hoverBehaviourFunc_Enter, hoverBehaviourFunc_Exit;
    public enum HoverBehaviour
    {
        Custom,
        Change_Color,
        Change_Color_Auto,
        Change_Image,
        Change_SetActive,
    }


    public Color hoverBehaviour_Color_Enter, hoverBehaviour_Color_Exit;

    private HoverBehaviour hoverBehaviourType = HoverBehaviour.Change_SetActive;
    private Image hoverBehaviour_Image;


    public static Button_UI currentlyActiveButton = null;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
       // Debug.Log("Chuột đi vào");
        mouseOver=true;
        mouseOverPerSecFuncTimer = 0;

       // if (hoverBehaviourFunc_Enter != null) hoverBehaviourFunc_Enter();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Chuột rời khỏi");
        mouseOver =false;

        //if (hoverBehaviourFunc_Exit != null) hoverBehaviourFunc_Exit();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Click xong");
        if(eventData.button==PointerEventData.InputButton.Left)
        {
            if(ClickFunc != null) { ClickFunc(); }
        }


        if (hoverBehaviourType == HoverBehaviour.Change_SetActive)
        {
            if (currentlyActiveButton != null && currentlyActiveButton != this)
            {
                currentlyActiveButton.hoverBehaviourFunc_Exit?.Invoke();
            }

            // Set lại active cho button hiện tại
            hoverBehaviourFunc_Enter?.Invoke();

            // Cập nhật button đang được active
            currentlyActiveButton = this;
        }


    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
       // Debug.Log("Đã nhấn chuột/trạm");
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
      //  Debug.Log("Thả chuột ra");
    }

    private void Awake()
    {
        Transform deepChild = FindDeepChild(transform, "Select");
        hoverBehaviour_Image=deepChild.GetComponent<Image>();
        hoverBehaviour_Image.gameObject.SetActive(false);
        SetHoverBehaviourType(hoverBehaviourType);
    }

    private void Update()
    {
        if (mouseOver)
        {
            mouseOverPerSecFuncTimer -= Time.unscaledDeltaTime;
            if (mouseOverPerSecFuncTimer <= 0)
            {
                mouseOverPerSecFuncTimer += 1f;
            }
        }
    }



    public void SetHoverBehaviourType(HoverBehaviour hoverBehaviourType)
    {

        this.hoverBehaviourType = hoverBehaviourType;
        switch(hoverBehaviourType)
        {
            case HoverBehaviour.Change_SetActive:
                {
                    hoverBehaviourFunc_Enter = delegate () { hoverBehaviour_Image.gameObject.SetActive(true); };
                    hoverBehaviourFunc_Exit = delegate () { hoverBehaviour_Image.gameObject.SetActive(false); };
                    break;
                }
        }
    }


    public static Transform FindDeepChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            Transform result = FindDeepChild(child, name);
            if (result != null) return result;
        }
        return null;
    }
}
