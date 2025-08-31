using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Select : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject guiSelect;
    public GameObject itemSelect;

    private int currentCategory=0;

    public static Button_Select Instance { get; private set; }
    public void Awake()
    {
        if(Instance!=null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }


    }


    public void ClearFullImageBackGround()
    {
        foreach (Transform child in guiSelect.transform)
        {

            Image img = child.GetComponent<Image>();
            if (img != null)
            {
                img.enabled = false;
            }
        }
    }

    public void ChangeCategory(int categoryIndex)
    {
        if (currentCategory == categoryIndex)
        {
            return;
        }

        currentCategory=categoryIndex;
        foreach(Transform child in itemSelect.transform)
        {
            child.gameObject.SetActive(false);
        }

        if (categoryIndex >= 0 && categoryIndex < itemSelect.transform.childCount)
        {
            // Bật đúng Object con tương ứng
            itemSelect.transform.GetChild(categoryIndex).gameObject.SetActive(true);
        }

    }







}
