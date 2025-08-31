using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem {


    private const string SAVE_EXTENSION = "txt";

    private static readonly string SAVE_FOLDER = Application.persistentDataPath + "/Saves/";
    private static bool isInit = false;


    public static void Init()
    {
        if (!isInit)
        {
            isInit = true;
            // Test if Save Folder exists
            if (!Directory.Exists(SAVE_FOLDER))
            {
                // Create Save Folder
                Directory.CreateDirectory(SAVE_FOLDER);
            }
        }
    }


    public static void Save(string fileName, string saveString, bool overwrite)
    {
        Init();
        string saveFileName = fileName;
        if (!overwrite)
        {
            // Make sure the Save Number is unique so it doesnt overwrite a previous save file
            int saveNumber = 1;
            while (File.Exists(SAVE_FOLDER + saveFileName + "." + SAVE_EXTENSION))
            {
                saveNumber++;
                saveFileName = fileName + "_" + saveNumber;
            }
            // saveFileName is unique
        }
        File.WriteAllText(SAVE_FOLDER + saveFileName + "." + SAVE_EXTENSION, saveString);
    }

    public static string Load(string fileName)
    {
        Init();
        if (File.Exists(SAVE_FOLDER + fileName + "." + SAVE_EXTENSION))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + fileName + "." + SAVE_EXTENSION);
            return saveString;
        }
        else
        {
            return null;
        }
    }


 

}
