    using System.Collections;
using System.Collections.Generic;

using UnityEngine;
//using static UnityEditorInternal.ReorderableList;
using static WallObject;

public class WallObject_Window : FloorEdgePlacedObject
{
    // Start is called before the first frame update
    private WindowObject windowObject;
    private List<WindowObject> windows = new List<WindowObject>();


    private DefaultPosition DefaultPosition;
    public List<DefaultPosition> defaultPositionArray=new List<DefaultPosition>();

    [SerializeField] private Dictionary<DefaultPosition, WindowObject> keyValuePairs;


    private void Awake()
    {
        keyValuePairs = new Dictionary<DefaultPosition,WindowObject>();
    }

    public void PlaceWindow(DefaultPosition defaultPosition, Window_DoorObjectTypeISO window_DoorObjectTypeISO)
    {
        Debug.Log("Start PlaceWindow()");

        if (defaultPosition == null)
        {
            Debug.LogError("PlaceWindow failed: defaultPosition is null");
            return;
        }

        if (window_DoorObjectTypeISO == null)
        {
            Debug.LogError("PlaceWindow failed: window_DoorObjectTypeISO is null");
            return;
        }

        if (window_DoorObjectTypeISO.prefab == null)
        {
            Debug.LogError("PlaceWindow failed: window_DoorObjectTypeISO.prefab is null");
            return;
        }

        Debug.Log("defaultPosition.isHave = " + defaultPosition.isHave);

        WindowObject windowObject = null;

        if (!defaultPosition.isHave)
        {
           // Debug.Log("Instantiating window object...");
            Transform windowObjectTransform = Instantiate(
                window_DoorObjectTypeISO.prefab.transform,
                defaultPosition.transform.position,
                defaultPosition.transform.rotation
            );

            windowObject = windowObjectTransform.GetComponent<WindowObject>();

            if (windowObject == null)
            {
                Debug.LogError("WindowObject component not found on instantiated prefab.");
            }
            else
            {
               // Debug.Log("WindowObject instantiated successfully.");
            }

            defaultPosition.isHave = true;
        }
        else
        {
           // Debug.Log("defaultPosition already has a window.");
        }

        if (windowObject != null)
        {
            if (keyValuePairs == null)
            {
                Debug.LogWarning("keyValuePairs Dictionary is null. Initializing...");
                keyValuePairs = new Dictionary<DefaultPosition, WindowObject>();
            }
            windowObject.onDestroyed += HandleWindowsDestroyed;
            keyValuePairs[defaultPosition] = windowObject;
            
           // Debug.Log("Assigned windowObject to keyValuePairs.");
        }
        else
        {
           // Debug.LogWarning("windowObject is null, skipping assignment to keyValuePairs.");
        }

        if (defaultPosition.windowHinge != null)
        {
           // Debug.Log("Configuring hinge joint...");

            Rigidbody connectedRb = defaultPosition.windowHinge.GetComponent<Rigidbody>();
            if (connectedRb == null)
            {
                connectedRb = defaultPosition.windowHinge.gameObject.AddComponent<Rigidbody>();
                connectedRb.isKinematic = true;
               // Debug.Log("Rigidbody added to windowHinge.");
            }

            if (windowObject != null)
            {
                windowObject.hingeJoint.connectedBody = connectedRb;
                Rigidbody rb = windowObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    Debug.Log("Set windowObject Rigidbody to kinematic.");
                }
                else
                {
                   // Debug.LogWarning("windowObject has no Rigidbody.");
                }
            }
        }
        else
        {
           // Debug.Log("defaultPosition.windowHinge is null, skipping hinge configuration.");
        }

       // Debug.Log("PlaceWindow() completed.");
    }



    public override void DestroyMySelf()
    {
        base.DestroyMySelf();

        foreach(DefaultPosition defPos in defaultPositionArray)
        {
            if (keyValuePairs != null && keyValuePairs.TryGetValue(defPos, out WindowObject windowObject))
            {
                if (windowObject != null)
                {
                    Destroy(windowObject.gameObject);
                }
            }
        }

    }

    private void HandleWindowsDestroyed(WindowObject destroyedWindow)
    {
        foreach (var pair in keyValuePairs)
        {
            if (pair.Value == destroyedWindow)
            {
                DefaultPosition pos = pair.Key;
                pos.isHave = false;
                keyValuePairs.Remove(pos);
                Debug.Log($"Window destroyed. Set isHave = false at {pos.name}");
                break;
            }
        }
    }
    public void Load(string data)
    {
        int i = 0;
        Debug.Log(data);

        DataWindowObjectSave dataWindowObjectSave = JsonUtility.FromJson<DataWindowObjectSave>(data);
        

        foreach(DefaultPosition defaultPos in defaultPositionArray)
        {

            string temp = dataWindowObjectSave.data[i];
            Debug.Log(temp);
            if (keyValuePairs != null && defaultPos != null && !string.IsNullOrEmpty(temp))
            {

                PlaceWindow(defaultPos, HouseBuildingSystemAssets.Instance.GetWindow_DoorObjectTypeISOFromName(temp));
                
            }
            i++;
        }
    }

    public WindowObjectSave Save()
    {
        FloorEdgePlacedObjectSave baseSave = base.Save();
        WindowObjectSave windowObjectSave = new WindowObjectSave
        {
            floorEdgePlacedObjectISOName = baseSave.floorEdgePlacedObjectISOName,
            data = baseSave.data,
            //doorTypeISOName = doorObject != null ? doorObject.GetWindow_DoorObjectTypeIsoName() : "",
            objectTypeName = "WallObject_Window",
            dataWindow = SaveData(),
        };
        return windowObjectSave;
    }

    public string SaveData()
    {

        List<string> data = new List<string>();
        foreach (DefaultPosition defaultPos in defaultPositionArray)
        {
            if (keyValuePairs != null && keyValuePairs.ContainsKey(defaultPos) && keyValuePairs[defaultPos] != null)
            {
                string isoName = keyValuePairs[defaultPos].GetWindow_DoorObjectTypeIsoName(); // lấy tên từ ISO
                data.Add(isoName);
            }
            else
            {
                data.Add(""); // không có cửa
            }
        }


        return JsonUtility.ToJson(new DataWindowObjectSave
        {
            data = data,
        });
    }



    [System.Serializable]
    public class WindowObjectSave : FloorEdgePlacedObjectSave
    {
        public bool hasDoor;
        public string windowTypeISOName; // tên hoặc ID cửa (lấy từ Window_DoorObjectTypeISO)
        public string dataWindow;
    }

    public class DataWindowObjectSave
    {
        public List<string> data;
    }

}
