using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class PanelSelect : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    public Action panelFun = null;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (panelFun != null) { panelFun(); }
            Debug.Log("CLick");
        }
    }

}
