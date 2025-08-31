using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class ItemSlot : MonoBehaviour
{
    // Start is called before the first frame update
    private Item item;
    public Image image;
    public TextMeshProUGUI text;
    public Item Item
    {
        get { return item; }
        set
        {
            item= value;
            if (item == null)
            {
                image.enabled=false;
            }
            else
            {
                image.sprite = item.icon;
                text.text= item.name;
                image.enabled=true;
            }
        }
    
    }

    private void OnValidate()
    {
        if (image == null)
        {
            Transform iconTransform = transform.Find("ItemIcon");
            Transform nameSpite = transform.Find("Count");
            if (iconTransform != null)
            {
                image = iconTransform.GetComponent<Image>();
                text = nameSpite.GetComponent<TextMeshProUGUI>();

              
            }
        }
    }
}
