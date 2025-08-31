using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FloorEdgePlacedObject;
using static FloorPlacedObject;
using static PlacedObjectISO;
using static WoodTriObject;

public class FloorEdgePlacedObject : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private FloorEdgeObjectTypeISO floorEdgeObjectTypeISO;


    //public List<WindowsPositon> 

    [SerializeField] private WoodTrimPosition woodTrimPosition;
    private WoodTriObject woodTriObject;
    private FloorEdgePlacedObject wallObject;

    public int gridLevel;
    public List<Vector2Int> point;

    public Action<FloorEdgePlacedObject> onDestroyed;
    public Action setPhysicsGrid;

    private void OnDestroy()
    {
        onDestroyed?.Invoke(this);
    }


    public static FloorEdgePlacedObject Create( FloorEdgeObjectTypeISO floorEdgeObjectTypeISO, FloorEdgePosition floorEdgePosition)
    {
        if (floorEdgeObjectTypeISO == null)
        {
            Debug.LogError("floorEdgeObjectTypeISO is null!");
            return null;
        }
        if (floorEdgeObjectTypeISO.prefab == null)
        {
            Debug.LogError("floorEdgeObjectTypeISO.prefab is null!");
            return null;
        }
        if (floorEdgePosition == null)
        {
            Debug.LogError("floorEdgePosition is null!");
            return null;
        }
        Transform newTransformFloorEdgePlacedObject=Instantiate(floorEdgeObjectTypeISO.prefab.transform,floorEdgePosition.transform.position,floorEdgePosition.transform.rotation);
/*        List<Vector2Int> gridPositionList = new List<Vector2Int>();*/


        floorEdgePosition.isHave = true;
        FloorEdgePlacedObject floorEdgePlacedObject=newTransformFloorEdgePlacedObject.GetComponent<FloorEdgePlacedObject>();     
        return floorEdgePlacedObject;
    }

    public static FloorEdgePlacedObject Create(FloorEdgeObjectTypeISO floorEdgeObjectTypeISO, WoodTrimPosition woodTrimPosition)
    {
        if (floorEdgeObjectTypeISO == null)
        {
            Debug.LogError("floorEdgeObjectTypeISO is null!");
            return null;
        }
        if (floorEdgeObjectTypeISO.prefab == null)
        {
            Debug.LogError("floorEdgeObjectTypeISO.prefab is null!");
            return null;
        }
        if (woodTrimPosition == null)
        {
            Debug.LogError("floorEdgePosition is null!");
            return null;
        }
        Transform newTransformFloorEdgePlacedObject = Instantiate(floorEdgeObjectTypeISO.prefab.transform, woodTrimPosition.transform.position, woodTrimPosition.transform.rotation);
        woodTrimPosition.isHave= true;
        FloorEdgePlacedObject floorEdgePlacedObject = newTransformFloorEdgePlacedObject.GetComponent<FloorEdgePlacedObject>();
        return floorEdgePlacedObject;
    }



    public string GetFloorEdgeObjectTypeIsoName()
    {
        return floorEdgeObjectTypeISO.name;
    }


    public void PlacedWoodTri(WoodTrimPosition woodTrimPosition, Basic_ObjectTypeISO basic_ObjectTypeISO)
    {
        Transform woodTriTransform = Instantiate(basic_ObjectTypeISO.prefab.transform, woodTrimPosition.transform.position, woodTrimPosition.transform.rotation);
        woodTrimPosition.isHave=true;

        WoodTriObject newWoodTriObject = woodTriTransform.GetComponent<WoodTriObject>();

        if (newWoodTriObject != null)
        {
            SetWoodTriObject(newWoodTriObject);
        }

        else
        {
            Debug.Log("Object chưa được gắn script WoodTriObject  ");

            return;
        }

    }

    public void PlacedWall(WoodTrimPosition woodTrimPosition, FloorEdgeObjectTypeISO floorEdgeObjectTypeISO)
    {
        /*if (floorEdgeObjectTypeISO == null)
        {
            Debug.LogError("floorEdgeObjectTypeISO is null! Cannot place wall.");
            return;
        }

        if (floorEdgeObjectTypeISO.prefab == null)
        {
            Debug.LogError("floorEdgeObjectTypeISO.prefab is null! Cannot place wall.");
            return;
        }
        if (woodTrimPosition == null)
        {
            Debug.LogError("woodTrimPosition is null! Cannot place wall.");
            return;
        }

        Transform wallTransform = Instantiate(floorEdgeObjectTypeISO.prefab.transform, woodTrimPosition.transform.position, woodTrimPosition.transform.rotation);*/
        
        FloorEdgePlacedObject wallObject;

        wallObject = Create(floorEdgeObjectTypeISO, woodTrimPosition);
        woodTrimPosition.isHave = true;
        if (wallObject != null) {

            SetWallObject(wallObject);

        }

    }

    //Load

    public void PlacedWoodTri(WoodTrimPosition woodTrimPosition, Basic_ObjectTypeISO basic_ObjectTypeISO, out WoodTriObject woodTriObject)
    {
        woodTriObject = WoodTriObject.Create(woodTrimPosition, basic_ObjectTypeISO);
        SetWoodTriObject(woodTriObject);
    }
    public void PlacedWall(WoodTrimPosition woodTrimPosition, FloorEdgeObjectTypeISO floorEdgeObjectTypeISO, out FloorEdgePlacedObject floorEdgePlacedObject)
    {
        floorEdgePlacedObject = Create(floorEdgeObjectTypeISO, woodTrimPosition);
        SetWallObject(floorEdgePlacedObject);
    }

    public void PlaceWoodStructure(AnglePosition anglePosition, Basic_ObjectTypeISO basic_ObjectType, out  WoodStructureObject woodStructureObject)
    {
        woodStructureObject = WoodStructureObject.Create(anglePosition, basic_ObjectType);

        //nothing
    }


    private void SetWoodTriObject(WoodTriObject woodTriObject)
    {
        this.woodTriObject = woodTriObject;
        woodTriObject.onDestroyed += HandleWoodTriDestroyed;
    }

    private void SetWallObject(FloorEdgePlacedObject wallObject)
    {
        this.wallObject = wallObject;
        wallObject.onDestroyed += HandleWallDestroyed;
    }

    private WoodTriObject GetWoodTriObject()
    {
        return this.woodTriObject;
    }


    public List<Vector2Int> GetGridPositionPhysic(Vector2Int offset, float euler)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (euler) {


            case 0:
                {
                   for(int i = 0; i < 7; i++)
                    {
                        gridPositionList.Add(offset+ new Vector2Int(i, 0));
                    }
                   break;
                }
            case 90:
                {
                    for (int i = 0; i < 7; i++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(0, i));
                    }
                    break;
                }
            case 180:
                {
                    for (int i = 0; i < 7; i++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(i, 6));
                    }
                    break;
                }
            case 270:
                {
                    for (int i = 0; i < 7; i++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(6, i));
                    }
                    break;
                }

        }
        return gridPositionList;

    }

    public void SetPointListAdd(List<Vector2Int> vector2Ints) {

        point = vector2Ints;
    }




    public virtual void DestroyMySelf()
    {
        //Destroy(this.gameObject);
        if (wallObject != null)
        {
            wallObject.DestroyMySelf();
        }
        if(woodTriObject != null)
        {
            woodTriObject.DestroyMySelf();
        }
        Destroy(this.gameObject);
    }

    private void HandleWallDestroyed(FloorEdgePlacedObject obj)
    {
        woodTrimPosition.isHave = false;
    }

    private void HandleWoodTriDestroyed(WoodTriObject obj)
    {
        woodTrimPosition.isHave = false;
    }

    public FloorEdgePlacedObjectSave SaveEdge(FloorEdgePlacedObject edgeObject)
    {
        if (edgeObject == null) return null;

        if (edgeObject is WallObject wallObject)
        {
            //Debug.Log("WallObjectYThat");
            return wallObject.Save();
        }
        if(edgeObject is WallObject_Window wallObject_Window)
        {
            return wallObject_Window.Save();
        }

        return edgeObject.Save();
    }

    public virtual FloorEdgePlacedObjectSave Save()
    {
        return new FloorEdgePlacedObjectSave
        {

            floorEdgePlacedObjectISOName = GetFloorEdgeObjectTypeIsoName(),
            data=this.SaveData(),
            objectTypeName= "FloorEdgePlacedObject",

        };
    }


    /*public string SaveData()
    {
        return JsonUtility.ToJson(new FloorEdgePlacedObjectData
        {
            wallObjectName= wallObject?.GetFloorEdgeObjectTypeIsoName() ?? "",
            woodTriObjectName=woodTriObject?.GetWoodTriuObjectTypeIsoName()?? "",
        });
    }*/

    public string SaveData()
    {
        return JsonUtility.ToJson(new FloorEdgePlacedObjectData
        {
            wallObjectName = wallObject?.SaveEdge(wallObject) ,
            woodTriObjectSave = woodTriObject?.Save(),
        });
    }



    public void Load(string saveString)
    {
        FloorEdgePlacedObjectData data = JsonUtility.FromJson<FloorEdgePlacedObjectData>(saveString);

        Debug.Log("Loading floor edge data: " + saveString);

        if (woodTrimPosition == null)
        {
            Debug.LogError("woodTrimPosition is NULL!");
            return;
        }

        if (!string.IsNullOrEmpty(data.woodTriObjectSave.woodTriObjectName))
        {
            Basic_ObjectTypeISO iso = HouseBuildingSystemAssets.Instance.GetBasic_ObjectTypeISOFromName(data.woodTriObjectSave.woodTriObjectName);
            if (iso == null)
            {
                Debug.LogError("Không tìm thấy Basic_ObjectTypeISO: " + data.woodTriObjectSave.woodTriObjectName);
            }
            else
            {
                PlacedWoodTri(this.woodTrimPosition, iso , out WoodTriObject woodTriObject);
                //woodTriObject.Load(saveString);
                WoodTriObjectSave woodTriObjectSave = data.woodTriObjectSave;
                string data2 = woodTriObjectSave.data;

                string data1=JsonUtility.ToJson(woodTriObjectSave);
                Debug.Log(data1);//{"woodTriObjectName":"WallWoodTri2_Left","data":"{\"roofObjectName\":\"Roof1_Piece1\"}"}
                Debug.Log(data2);//{"roofObjectName":"Roof1_Piece1"}
                if (!string.IsNullOrEmpty(data2))
                {
                    woodTriObject.Load(data2);
                }
                //woodTriObject.Load(data2);
                /*string data = JsonUtility.FromJson(woodTriObjectSave);
                woodTriObject.Load(data);*/
            }
        }

        if (!string.IsNullOrEmpty(data.wallObjectName.floorEdgePlacedObjectISOName))
        {

            Debug.Log(data.wallObjectName.floorEdgePlacedObjectISOName);
            var wallISO = HouseBuildingSystemAssets.Instance.GetFloorEdgeObjectTypeISOFromName(data.wallObjectName.floorEdgePlacedObjectISOName);
            if (wallISO == null)
            {
                Debug.LogError("Không tìm thấy FloorEdgeObjectTypeISO: " + data.wallObjectName.floorEdgePlacedObjectISOName);
            }
            else
            {
                FloorEdgePlacedObjectSave temp = data.wallObjectName;

                string floorEdgePlacedObjectData = temp.data;
                PlacedWall(this.woodTrimPosition, wallISO, out FloorEdgePlacedObject floorEdgePlacedObject);
                floorEdgePlacedObject.Load(floorEdgePlacedObjectData);
                //floorEdgePlacedObject.Load();
            }
        }
    }



    [System.Serializable]

    public class FloorEdgePlacedObjectSave{
        public string floorEdgePlacedObjectISOName;
        public string data;
        public string objectTypeName; /// Phân loại
    }


    public class FloorEdgePlacedObjectData
    {
        public WoodTriObjectSave woodTriObjectSave;
        public FloorEdgePlacedObjectSave wallObjectName;
    }
}
