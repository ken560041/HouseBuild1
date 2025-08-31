using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDeleteObjectGhost : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject _current;
    public static HouseDeleteObjectGhost Instance { get; private set; }

    private bool _isGhost=false;
    private GameObject current

    {
        get => _current;
        set
        {
            if (_current == value) return; // Không làm gì nếu giống nhau

            if (_current != null && _current != value)
            {
                if (_current.name == "WoodStairs2(Clone)")
                {
                    SetLayerRecursive(_current, LayerMask.NameToLayer("Obstacle"));
                }
                else
                {
                    SetLayerRecursive(_current, LayerMask.NameToLayer("Default"));
                }
            }

            _current = value;

            if (_current != null)
            {
                SetLayerRecursive(_current, LayerMask.NameToLayer("Ghost"));
            }

            // Tùy chọn gọi sau cùng nếu cần
            // GridBuildingSystem.Instance.RefreshDeleteObjectType(); 
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        GridBuildingSystem.Instance.OnSelectedDeleteChanged += Instante_OnDeleteChanged;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = GridBuildingSystem.Instance.GetMouseWorldPosition();   

        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 25f);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, GridBuildingSystem.Instance.GetLooseObjectEulerY(), 0), Time.deltaTime * 15f);
        //GridBuildingSystem.Instance.OnSelectedDeleteChanged += Instante_OnDeleteChanged;
        if (GridBuildingSystem.Instance.isDemolishActive)
        {
            
                GetObjectGhost();
            
        }
        else
        {
            current = null;
        }
    }


    private void Instante_OnDeleteChanged(object sander, System.EventArgs e)
    {
       // RefeshVisual();
       
       
    }

    public void RefeshVisual()
    {
        switch (GridBuildingSystem.Instance.deletePlaceObjectType)
        {
            case GridBuildingSystem.PlaceObjectType.GridObject:
                {
                    TestGetFloorPlacedObjectGhost();
                    Debug.Log("hoat dong");
                    break;
                }
        }

    }

    public void GetObjectGhost()
    {
        switch (GridBuildingSystem.Instance.deletePlaceObjectType)
        {
            case GridBuildingSystem.PlaceObjectType.GridObject:
                {
                    GetGridObjectGhost();
                    //Debug.Log("hoat dong");
                    break;
                }
            case GridBuildingSystem.PlaceObjectType.EdgeObject:
                {
                    GetEdgeObjectGhost();
                    break;
                }
            case GridBuildingSystem.PlaceObjectType.WoodTriObject:
                {
                    GetWoodTriObjectGhost();
                    break;
                }
            case GridBuildingSystem.PlaceObjectType.RoofObject:
                {
                    GetRoofObjectGhost();
                    break;
                }
            case GridBuildingSystem.PlaceObjectType.DoorObject:
                {
                    GetDoorObjectGhost();
                    break;
                }
            case GridBuildingSystem.PlaceObjectType.WindowObject:
                {
                    GetWindowObjectGhost();
                    break;
                }
        }

    }



    private void GetGridObjectGhost()
    {
        //FloorPlacedObject floorPlacedObject= GridBuildingSystem.Instance.GetFloorPlacedObject();
        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
        Vector3 ghostPosition = transform.position;

        var gridObject = GridBuildingSystem.Instance.selectGrid.GetGridObject(mousePosition);
        if (gridObject == null)
        {
           // Debug.LogWarning("⚠️ Không có GridObject tại vị trí ghost");
            current = null;
            return;
        }
        if (gridObject != null)
        {
            PlacedObject placedObject = gridObject.GetPlacedObject();// hoạt động

            if (placedObject==null)
            {
                if (current != null)
                {
                    current = null;
                    Debug.LogWarning("⚠️ Không có placedObject tại vị trí ghost");
                    return;
                }
            }

            if (placedObject != null)
            {
                FloorPlacedObject floorPlacedObject = placedObject as FloorPlacedObject;
                if (floorPlacedObject != null)
                {
                    GameObject newCurrent = floorPlacedObject.gameObject;

                    if (current != newCurrent)
                    {
                        current = newCurrent;
                    }
                   
                }
                else
                {
                    if (current != null)
                    {
                        current = null;
                        Debug.Log("Không còn object nào dưới ghost");
                    }

                }
            }
        }
    }

    private void GetEdgeObjectGhost()
    {
        Vector3 ghostPosition = transform.position;
        FloorEdgePlacedObject floorEdgePlacedObject = GridBuildingSystem.Instance.GetFloorEdgePlacedObject();
        if( floorEdgePlacedObject != null )
        {
            //Debug.Log("COs ton tai");
            GameObject newCurrent=floorEdgePlacedObject.gameObject;
            if(current!= newCurrent )
            {
                current = newCurrent;
            }
        }
        else
        {
            if(current != null)
            {
                current=null;
            }
            //Debug.Log("Khong  ton tai");
        }
    }
    private void TestGetFloorPlacedObjectGhost()
    {
        FloorPlacedObject floorPlacedObject=GridBuildingSystem.Instance.GetFloorPlacedObject();
        if (floorPlacedObject != null)
        {
            Debug.Log("COs ton tai");
            GameObject newCurrent = floorPlacedObject.gameObject;
            if (current != newCurrent)
            {
                current = newCurrent;
            }
        }
        else
        {
            if (current != null)
            {
                current = null;
            }
            Debug.Log("Khong  ton tai");
        }
    }

    //private void GetF

    private void GetWoodTriObjectGhost()
    {
        WoodTriObject woodTriObject=GridBuildingSystem.Instance.GetWoodTriObject();

        if (woodTriObject != null)
        {

            Debug.Log("lay dc woodtri");
            GameObject newCurrent=woodTriObject.gameObject;
            if (current != newCurrent)
            {
                current = newCurrent;
            }
        }
        else { 
            if (current != null)
            {
                current =null;
            } 
        }
    }


    private void GetRoofObjectGhost()
    {
        RoofObject roofObject= GridBuildingSystem.Instance.GetRoofObject();

        if (roofObject != null)
        {

            Debug.Log("lay dc woodtri");
            GameObject newCurrent = roofObject.gameObject;
            if (current != newCurrent)
            {
                current = newCurrent;
            }
        }
        else
        {
            if (current != null)
            {
                current = null;
            }
        }
    }

    private void GetDoorObjectGhost()
    {
        DoorObject doorObject=GridBuildingSystem.Instance.GetDoorObject();
        if (doorObject != null)
        {

            Debug.Log("lay dc woodtri");
            GameObject newCurrent = doorObject.gameObject;
            if (current != newCurrent)
            {
                current = newCurrent;
            }
        }
        else
        {
            if (current != null)
            {
                current = null;
            }
        }
    }

    private void GetWindowObjectGhost()
    {
        WindowObject windowObject= GridBuildingSystem.Instance.GetWindowObject();
        if (windowObject != null)
        {

            Debug.Log("lay dc woodtri");
            GameObject newCurrent = windowObject.gameObject;
            if (current != newCurrent)
            {
                current = newCurrent;
            }
        }
        else
        {
            if (current != null)
            {
                current = null;
            }
        }
    }
    void ChanggeLayerDeleteObject(int layer)
    {
            //Dổi layer
    }


    private void SetLayerRecursive(GameObject targetObject, int layer)
    {
        targetObject.layer = layer;

        foreach (Transform child in targetObject.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }




    }
