using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectPathSave : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform content;


    public void Setup()
    {

        List<string> savePaths = HouseSaveManager.Instance.pathArray;
        for (int i = 0; i < content.childCount; i++)
        {
            Transform child = content.GetChild(i);
            GameObject buttonObj = child.gameObject;
            PanelSelect panelSelect = buttonObj.GetComponent<PanelSelect>();

            if (panelSelect != null && i < savePaths.Count)
            {
                string saveName = savePaths[i]; // lấy path tương ứng

                panelSelect.panelFun = () =>
                {
                    Debug.Log("Click bản ghi: " + saveName);

                    HouseSaveManager.Instance.currentPath= saveName;
                    // PlayerPrefs.SetString("CurrentSaveName", saveName);
                    SceneManager.LoadScene(0); // Load game
                };

            }


        }
    }
}
