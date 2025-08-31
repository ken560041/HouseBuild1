using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class HouseSaveManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static HouseSaveManager Instance { get; private set; }
    public List<string> pathArray;
    public string currentPath;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // tránh trùng lặp
        }
        LoadPath();
    }

    public void Add(string name)
    {
        pathArray.Add(name);
        Debug.Log(name);
    }
    public void Remove(string name)
    {
        pathArray.Remove(name); 
    }

    
    public void SavePath()
    {
        HouseBuildingSavePath data = new HouseBuildingSavePath
        {
            Listpath = pathArray
        };

        string json = JsonUtility.ToJson(data, true); // true = pretty print (dễ đọc)
        SaveSystem.Save("HouseBuildingPath", json, true); // true la overidle
    }

    public void LoadPath()
    {
        string json = SaveSystem.Load("HouseBuildingPath");

        if (!string.IsNullOrEmpty(json))
        {
            HouseBuildingSavePath data = JsonUtility.FromJson<HouseBuildingSavePath>(json);
            pathArray = data.Listpath ?? new List<string>();
        }
        else
        {
            pathArray = new List<string>(); // nếu không có file thì khởi tạo rỗng
        }
    }


    [System.Serializable]
    public class HouseBuildingSavePath
    {
        public List <string> Listpath;
    }
}
