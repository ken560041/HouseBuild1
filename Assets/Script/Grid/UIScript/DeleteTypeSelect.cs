using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeleteTypeSelect : MonoBehaviour
{
    // Start is called before the first frame update

    private TextMeshProUGUI textMeshPro;
    void Start()
    {
        GridBuildingSystem.Instance.OnSelectedDeleteChanged += GridBuildSystem_OnSelectedDeleteChanged;
    }

    // Update is called once per frame
    void Update()
    {
        bool isActive = GridBuildingSystem.Instance.isDemolishActive;
        textMeshPro.gameObject.SetActive(isActive);
    }

    private void Awake()
    {
        textMeshPro = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        
    }

    private void GridBuildSystem_OnSelectedDeleteChanged(object sender, System.EventArgs e)
    {
        
        textMeshPro.text = ""+ GridBuildingSystem.Instance.GetDeletePlaceObjectType().ToString();
        
        
    }
    
}
