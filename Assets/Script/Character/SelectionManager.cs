using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    /*public GameObject interaction_Info_UI; // UI hiển thị thông tin
    public TMP_Text interaction_text;     // TextMeshPro component
    public bool onTarget;


    public GameObject selectObject;*/
    public static SelectionManager Instance { get; set; }

    public GameObject _buttonInteractor;
    public Transform _interactorConnect;


    private List<GameObject> buttonList=new List<GameObject>();

    

    private void Start()
    {
       
        /*interaction_text = interaction_Info_UI.GetComponent<TMP_Text>();

        if (interaction_text == null)
        {
            Debug.LogError("TMP_Text component not found on interaction_Info_UI!");
        }*/
    }



    private void Awake()
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

    public void RenderButton(List<InteractableObject> interactables)
    {

        while(buttonList.Count<interactables.Count)
        {
            GameObject _obj= Instantiate(_buttonInteractor, _interactorConnect);
            buttonList.Add(_obj);
        }


        for (int i = interactables.Count; i < buttonList.Count; i++)
        {
            buttonList[i].SetActive(false);
        }

        UpdateSelectInteractor(interactables, 0);

    }

    public void UpdateSelectInteractor(List<InteractableObject> _interactables, int _selectedIndex)
    {
        for(int i=0; i < _interactables.Count;i++) { 
        
            IInteractable _interactable = _interactables[i]._collider.GetComponent<IInteractable>();
            if (_interactable != null)
            {
                

                var _interactorName = buttonList[i].transform.Find("UI_Interactor").GetComponent<TextMeshProUGUI>();
                var _imageSelect = buttonList[i].transform.Find("Image_Select").GetComponent<Image>();
                    _interactorName.text = _interactable.GetInteractText();
                    _imageSelect.gameObject.SetActive(i == _selectedIndex); // Chỉ bật Image_Select ở button được chọn
                    buttonList[i].SetActive(true);

                }

        }
    }

    public void FreeButton(){
        foreach (Transform _button in _interactorConnect)
        {
            Destroy(_button.gameObject);

        }
    }

}
