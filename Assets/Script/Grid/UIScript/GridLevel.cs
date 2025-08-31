using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridLevel : MonoBehaviour
{
    // Start is called before the first frame update

    private TextMeshProUGUI textMeshPro;
    void Start()
    {

        GridBuildingSystem.Instance.OnActiveGridLevelChanged += GridBuildingSystem_OnActiveGridLevelChanged;
        
    }

    private void Awake()
    {
        textMeshPro=transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        textMeshPro.text = "GridLevel" +1;
    }


    private void GridBuildingSystem_OnActiveGridLevelChanged(object sender, System.EventArgs e)
    {
        textMeshPro.text = "Grid Level " + (GridBuildingSystem.Instance.GetActiveGridLevel() + 1);
    }
}
