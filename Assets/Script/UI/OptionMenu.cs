using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject optionMenu;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Exit()
    {
        SceneManager.LoadScene(1);
        //SceneManager.SetActiveScene(0);
       // HouseSaveManager.Instance.currentPath = null;
    }

    public void Load()
    {
        GridBuildingSystem.Instance.Load();
    }
    public void Save()
    {
        GridBuildingSystem.Instance.Save();
    }

    public void Close()
    {
        optionMenu.SetActive(false);
    }
}
