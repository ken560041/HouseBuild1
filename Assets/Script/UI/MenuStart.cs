using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuStart : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject continuePanel; // panel hiện lên khi nhấn Continue
    public GameObject startPanel;
    public Transform content;        // content của ScrollRect
    public GameObject buttonPrefab;  // prefab nút bản ghi


    public SelectPathSave selectPathSave;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }




    public void Play()
    {

        string newSaveName = "save_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss");

        
        HouseSaveManager.Instance.Add(newSaveName);
        HouseSaveManager.Instance.SavePath();
        HouseSaveManager.Instance.currentPath = newSaveName;
        SceneManager.LoadScene(0);
    }


    public void Continue()
    {
        continuePanel.SetActive(true);
        startPanel.SetActive(false);
        // Xóa các button cũ nếu có
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        List<string> savePaths = HouseSaveManager.Instance.pathArray;

        foreach (string path in savePaths)
        {
            GameObject btn = Instantiate(buttonPrefab, content);
            TextMeshProUGUI text = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = path;
            }
        }

        selectPathSave.Setup();

    }

    public void Exit()
    {
        Application.Quit();
    }

    public void CloseContinue()
    {
        continuePanel.SetActive(false);
        startPanel.SetActive(true);
    }

}
