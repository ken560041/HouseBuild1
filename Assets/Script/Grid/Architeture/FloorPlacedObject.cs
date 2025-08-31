using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static FloorEdgePlacedObject;
using static WallObject;
using static WallObject_Window;

public class FloorPlacedObject : PlacedObject
{

    public enum Edge
    {
        Up, Down, Left, Right
    }

    // Start is called before the first frame update



    [SerializeField] private FloorEdgePosition upFloorEdgePosition;
    [SerializeField] private FloorEdgePosition downFloorEdgePosition;
    [SerializeField] private FloorEdgePosition leftFloorEdgePosition;
    [SerializeField] private FloorEdgePosition rightFloorEdgePosition;



    private FloorEdgePlacedObject upFloorEdgePlacedObject;
    private FloorEdgePlacedObject downFloorEdgePlacedObject;
    private FloorEdgePlacedObject leftFloorEdgePlacedObject;
    private FloorEdgePlacedObject rightFloorEdgePlacedObject;



    public void PlaceEdge(Edge edge, FloorEdgeObjectTypeISO floorEdgeObjectTypeISO)
    {


        FloorEdgePosition floorEdgePosition = GetFloorEdgePosition(edge);
        /*Transform floorEdgeObjectTransform= Instantiate(floorEdgeObjectTypeISO.prefab.transform, floorEdgePosition.transform.position,
            floorEdgePosition.transform.rotation);
        FloorEdgePlacedObject currentFloorEdgePlacedObject=GetFloorEdgePlacedObject(edge);*/

        FloorEdgePlacedObject floorEdgePlacedObjecttest = FloorEdgePlacedObject.Create(floorEdgeObjectTypeISO, floorEdgePosition);
        FloorEdgePlacedObject currentFloorEdgePlacedObject = GetFloorEdgePlacedObject(edge);
        if (currentFloorEdgePlacedObject != null)
        {

            Destroy(currentFloorEdgePlacedObject.gameObject);
        }

        SetFloorEdgePlacedObject(edge, floorEdgePlacedObjecttest);

    }

    //opp-> FloorEdgeObjectTypeISO là tường cần đặt vào các cạnh 


    public void PlaceEdge(Edge edge, FloorEdgeObjectTypeISO floorEdgeObjectTypeISO, out FloorEdgePlacedObject floorEdgePlacedObject)
    {
        FloorEdgePosition floorEdgePosition = GetFloorEdgePosition(edge);
        floorEdgePlacedObject = FloorEdgePlacedObject.Create(floorEdgeObjectTypeISO, floorEdgePosition);
        FloorEdgePlacedObject currentFloorEdgePlacedObject = GetFloorEdgePlacedObject(edge);
        if (currentFloorEdgePlacedObject != null)
        {

            Destroy(currentFloorEdgePlacedObject.gameObject);
        }
        SetFloorEdgePlacedObject(edge, floorEdgePlacedObject);
    }
    public FloorEdgePosition GetFloorEdgePosition(Edge edge)
    {
        switch (edge)
        {
            default:
            case Edge.Up: return upFloorEdgePosition;
            case Edge.Down: return downFloorEdgePosition;
            case Edge.Left: return leftFloorEdgePosition;
            case Edge.Right: return rightFloorEdgePosition;


        }


    }


    private void SetFloorEdgePlacedObject(Edge edge, FloorEdgePlacedObject floorEdgePlacedObject)
    {
        switch (edge)
        {
            default:
            case Edge.Up:
                upFloorEdgePlacedObject = floorEdgePlacedObject;
                upFloorEdgePlacedObject.onDestroyed += HandleEdgeDestroyed;
                GetFloorEdgePosition(edge).isHave=true;
                break;

            case Edge.Left:
                leftFloorEdgePlacedObject = floorEdgePlacedObject;
                leftFloorEdgePlacedObject.onDestroyed += HandleEdgeDestroyed;
                GetFloorEdgePosition(edge).isHave = true;
                break;
            case Edge.Right:
                rightFloorEdgePlacedObject = floorEdgePlacedObject;
                rightFloorEdgePlacedObject.onDestroyed += HandleEdgeDestroyed;
                GetFloorEdgePosition(edge).isHave = true;
                break;
            case Edge.Down:
                downFloorEdgePlacedObject = floorEdgePlacedObject;
                downFloorEdgePlacedObject.onDestroyed += HandleEdgeDestroyed;
                GetFloorEdgePosition(edge).isHave = true;
                break;
        }
    }


    public FloorEdgePlacedObject GetFloorEdgePlacedObject(Edge edge)
    {
        switch (edge)
        {

            default:
            case Edge.Up:
                return upFloorEdgePlacedObject;
            case Edge.Down:
                return downFloorEdgePlacedObject;
            case Edge.Left:
                return leftFloorEdgePlacedObject;
            case Edge.Right:
                return rightFloorEdgePlacedObject;
        }
    }



    public override void DestroyMySelf()
    {
        if (upFloorEdgePlacedObject != null)
        {
            upFloorEdgePlacedObject.DestroyMySelf();

        }
        if (downFloorEdgePlacedObject != null) downFloorEdgePlacedObject.DestroyMySelf();
        if (leftFloorEdgePlacedObject != null) leftFloorEdgePlacedObject.DestroyMySelf();
        if (rightFloorEdgePlacedObject != null) rightFloorEdgePlacedObject.DestroyMySelf();
        base.DestroyMySelf();
    }


    private void HandleEdgeDestroyed(FloorEdgePlacedObject destroyedObject)
    {
        if (destroyedObject == upFloorEdgePlacedObject)
        {
            upFloorEdgePlacedObject = null;
            upFloorEdgePosition.isHave = false;
        }
        else if (destroyedObject == downFloorEdgePlacedObject)
        {
            downFloorEdgePlacedObject = null;
            downFloorEdgePosition.isHave = false;
        }
        else if (destroyedObject == leftFloorEdgePlacedObject)
        {
            leftFloorEdgePlacedObject = null;
            leftFloorEdgePosition.isHave = false;
        }
        else if (destroyedObject == rightFloorEdgePlacedObject)
        {
            rightFloorEdgePlacedObject = null;
            rightFloorEdgePosition.isHave = false;
        }
    }


    public string Save()
    {
        return JsonUtility.ToJson(new FloorSaveObject
        {
            upEdgeObjectName = JsonUtility.ToJson(upFloorEdgePlacedObject?.SaveEdge(upFloorEdgePlacedObject)),
            downEdgeObjectName = JsonUtility.ToJson(downFloorEdgePlacedObject?.SaveEdge(downFloorEdgePlacedObject)),
            leftEdgeObjectName = JsonUtility.ToJson(leftFloorEdgePlacedObject?.SaveEdge(leftFloorEdgePlacedObject)),
            rightEdgeObjectName = JsonUtility.ToJson(rightFloorEdgePlacedObject?.SaveEdge(rightFloorEdgePlacedObject)),
        });
    }


    public void Load(string saveString)
    {
        FloorSaveObject floorSaveObject = JsonUtility.FromJson<FloorSaveObject>(saveString);


        string up = floorSaveObject.upEdgeObjectName;
        string down = floorSaveObject.downEdgeObjectName;
        string left = floorSaveObject.leftEdgeObjectName;
        string right = floorSaveObject.rightEdgeObjectName;

        Debug.Log(floorSaveObject.downEdgeObjectName);

        if (!string.IsNullOrEmpty(floorSaveObject.upEdgeObjectName) && !string.IsNullOrEmpty(JsonUtility.FromJson<FloorEdgePlacedObjectSave>(floorSaveObject.upEdgeObjectName).floorEdgePlacedObjectISOName))
        {



            Debug.Log(JsonUtility.FromJson<FloorEdgePlacedObjectSave>(floorSaveObject.upEdgeObjectName).floorEdgePlacedObjectISOName);
            PlaceEdge(Edge.Up, HouseBuildingSystemAssets.Instance.GetFloorEdgeObjectTypeISOFromName(JsonUtility.FromJson<FloorEdgePlacedObjectSave>(floorSaveObject.upEdgeObjectName).floorEdgePlacedObjectISOName), out FloorEdgePlacedObject floorEdgePlacedObject);

            FloorEdgePlacedObjectSave floorEdgePlacedObjectSave = JsonUtility.FromJson<FloorEdgePlacedObjectSave>(floorSaveObject.upEdgeObjectName);
            string floorEdgePlacedObjectData = floorEdgePlacedObjectSave.data;
            string data = JsonUtility.ToJson(floorEdgePlacedObjectSave);
            Debug.Log(data); //{"floorEdgePlacedObjectISOName":"Wall2","data":"{\"woodTriObjectName\":\"\",\"wallObjectName\":\"Wall2\"}"}
            if (1 == 1)
            {

                data = JsonUtility.ToJson(floorEdgePlacedObjectData);
                //Debug.Log(floorEdgePlacedObjectData);


                if (!string.IsNullOrEmpty(floorEdgePlacedObjectData))
                {

                    floorEdgePlacedObject.Load(floorEdgePlacedObjectData);
                    if (floorEdgePlacedObjectSave.objectTypeName == "WallObject")
                    {
                        WallObject.WallObjectSave wallObjectSave = JsonUtility.FromJson<WallObjectSave>(floorSaveObject.upEdgeObjectName);

                        if (!string.IsNullOrEmpty(wallObjectSave.doorTypeISOName))
                        {
                            Debug.Log(wallObjectSave.doorTypeISOName);
                            ((WallObject)floorEdgePlacedObject).Load(wallObjectSave.doorTypeISOName);
                        }
                    }

                    else if (floorEdgePlacedObjectSave.objectTypeName == "WallObject_Window")
                    {
                        WallObject_Window.WindowObjectSave windowObjectSave = JsonUtility.FromJson<WindowObjectSave>(floorSaveObject.upEdgeObjectName);
                        if (!string.IsNullOrEmpty(windowObjectSave.windowTypeISOName))
                        {
                            ((WallObject_Window)floorEdgePlacedObject).Load(windowObjectSave.dataWindow);
                        }
                    }
                }



            }



        }
        if (!string.IsNullOrEmpty(floorSaveObject.downEdgeObjectName) && !string.IsNullOrEmpty(JsonUtility.FromJson<FloorEdgePlacedObjectSave>(floorSaveObject.downEdgeObjectName).floorEdgePlacedObjectISOName))
        {
            PlaceEdge(Edge.Down, HouseBuildingSystemAssets.Instance.GetFloorEdgeObjectTypeISOFromName(JsonUtility.FromJson<FloorEdgePlacedObjectSave>(floorSaveObject.downEdgeObjectName).floorEdgePlacedObjectISOName), out FloorEdgePlacedObject floorEdgePlacedObject);

            FloorEdgePlacedObjectSave floorEdgePlacedObjectSave = JsonUtility.FromJson<FloorEdgePlacedObjectSave>(floorSaveObject.downEdgeObjectName);
            string floorEdgePlacedObjectData = floorEdgePlacedObjectSave.data;
            string data = JsonUtility.ToJson(floorEdgePlacedObjectSave);
            Debug.Log(data); //{"floorEdgePlacedObjectISOName":"Wall2","data":"{\"woodTriObjectName\":\"\",\"wallObjectName\":\"Wall2\"}"}
            if (1 == 1)
            {

                data = JsonUtility.ToJson(floorEdgePlacedObjectData);
                // Debug.Log(floorEdgePlacedObjectData);

                if (!string.IsNullOrEmpty(floorEdgePlacedObjectData))
                {

                    floorEdgePlacedObject.Load(floorEdgePlacedObjectData);
                    if (floorEdgePlacedObjectSave.objectTypeName == "WallObject")
                    {
                        WallObject.WallObjectSave wallObjectSave = JsonUtility.FromJson<WallObjectSave>(floorSaveObject.downEdgeObjectName);

                        if (!string.IsNullOrEmpty(wallObjectSave.doorTypeISOName))
                        {
                            Debug.Log(wallObjectSave.doorTypeISOName);
                            ((WallObject)floorEdgePlacedObject).Load(wallObjectSave.doorTypeISOName);
                        }
                    }
                    else if (floorEdgePlacedObjectSave.objectTypeName == "WallObject_Window")
                    {
                        WallObject_Window.WindowObjectSave windowObjectSave = JsonUtility.FromJson<WindowObjectSave>(floorSaveObject.downEdgeObjectName);
                        Debug.Log(windowObjectSave.dataWindow);
                        if (!string.IsNullOrEmpty(windowObjectSave.dataWindow))
                        {
                            {

                                ((WallObject_Window)floorEdgePlacedObject).Load(windowObjectSave.dataWindow);
                            }
                        }
                    }




                }
            }
        }
        if (!string.IsNullOrEmpty(floorSaveObject.leftEdgeObjectName) && !string.IsNullOrEmpty(JsonUtility.FromJson<FloorEdgePlacedObjectSave>(floorSaveObject.leftEdgeObjectName).floorEdgePlacedObjectISOName))
        {
            PlaceEdge(Edge.Left, HouseBuildingSystemAssets.Instance.GetFloorEdgeObjectTypeISOFromName(JsonUtility.FromJson<FloorEdgePlacedObjectSave>(floorSaveObject.leftEdgeObjectName).floorEdgePlacedObjectISOName), out FloorEdgePlacedObject floorEdgePlacedObject);

            FloorEdgePlacedObjectSave floorEdgePlacedObjectSave = JsonUtility.FromJson<FloorEdgePlacedObjectSave>(floorSaveObject.leftEdgeObjectName);
            string floorEdgePlacedObjectData = floorEdgePlacedObjectSave.data;
            string data = JsonUtility.ToJson(floorEdgePlacedObjectSave);
            Debug.Log(data); //{"floorEdgePlacedObjectISOName":"Wall2","data":"{\"woodTriObjectName\":\"\",\"wallObjectName\":\"Wall2\"}"}
            if (1 == 1)
            {

                data = JsonUtility.ToJson(floorEdgePlacedObjectData);
                //Debug.Log(data);


                if (!string.IsNullOrEmpty(floorEdgePlacedObjectData))
                {

                    floorEdgePlacedObject.Load(floorEdgePlacedObjectData);

                    if (floorEdgePlacedObjectSave.objectTypeName == "WallObject")
                    {
                        WallObject.WallObjectSave wallObjectSave = JsonUtility.FromJson<WallObjectSave>(floorSaveObject.leftEdgeObjectName);

                        if (!string.IsNullOrEmpty(wallObjectSave.doorTypeISOName))
                        {
                            Debug.Log(wallObjectSave.doorTypeISOName);
                            ((WallObject)floorEdgePlacedObject).Load(wallObjectSave.doorTypeISOName);
                        }
                    }

                    else if (floorEdgePlacedObjectSave.objectTypeName == "WallObject_Window")
                    {
                        WallObject_Window.WindowObjectSave windowObjectSave = JsonUtility.FromJson<WindowObjectSave>(floorSaveObject.leftEdgeObjectName);
                        if (!string.IsNullOrEmpty(windowObjectSave.dataWindow))
                        {
                            ((WallObject_Window)floorEdgePlacedObject).Load(windowObjectSave.dataWindow);
                        }
                    }
                }
                



            }
        }

        if (!string.IsNullOrEmpty(floorSaveObject.rightEdgeObjectName) && !string.IsNullOrEmpty(JsonUtility.FromJson<FloorEdgePlacedObjectSave>(floorSaveObject.rightEdgeObjectName).floorEdgePlacedObjectISOName))
        {
            PlaceEdge(Edge.Right, HouseBuildingSystemAssets.Instance.GetFloorEdgeObjectTypeISOFromName(JsonUtility.FromJson<FloorEdgePlacedObjectSave>(floorSaveObject.rightEdgeObjectName).floorEdgePlacedObjectISOName), out FloorEdgePlacedObject floorEdgePlacedObject);

            FloorEdgePlacedObjectSave floorEdgePlacedObjectSave = JsonUtility.FromJson<FloorEdgePlacedObjectSave>(floorSaveObject.rightEdgeObjectName);
            string floorEdgePlacedObjectData = floorEdgePlacedObjectSave.data;
            string data = JsonUtility.ToJson(floorEdgePlacedObjectSave);
            Debug.Log(data); //{"floorEdgePlacedObjectISOName":"Wall2","data":"{\"woodTriObjectName\":\"\",\"wallObjectName\":\"Wall2\"}"}
            if (1 == 1)
            {

                data = JsonUtility.ToJson(floorEdgePlacedObjectData);
                // Debug.Log(data);


                if (!string.IsNullOrEmpty(floorEdgePlacedObjectData))
                {

                    floorEdgePlacedObject.Load(floorEdgePlacedObjectData);

                    if (floorEdgePlacedObjectSave.objectTypeName == "WallObject")
                    {
                        WallObject.WallObjectSave wallObjectSave = JsonUtility.FromJson<WallObjectSave>(floorSaveObject.rightEdgeObjectName);

                        if (!string.IsNullOrEmpty(wallObjectSave.doorTypeISOName))
                        {
                            Debug.Log(wallObjectSave.doorTypeISOName);
                            ((WallObject)floorEdgePlacedObject).Load(wallObjectSave.doorTypeISOName);
                        }
                    }

                    else if (floorEdgePlacedObjectSave.objectTypeName == "WallObject_Window")
                    {
                        WallObject_Window.WindowObjectSave windowObjectSave = JsonUtility.FromJson<WindowObjectSave>(floorSaveObject.rightEdgeObjectName);
                        if (!string.IsNullOrEmpty(windowObjectSave.dataWindow))
                        {
                            ((WallObject_Window)floorEdgePlacedObject).Load(windowObjectSave.dataWindow);
                        }
                    }

                }
                



            }
        }


    }


    [System.Serializable]
    public class FloorSaveObject
    {
        /*public FloorEdgePlacedObject.FloorEdgePlacedObjectSave upEdgeObjectName;
        public FloorEdgePlacedObject.FloorEdgePlacedObjectSave downEdgeObjectName;
        public FloorEdgePlacedObject.FloorEdgePlacedObjectSave leftEdgeObjectName;
        public FloorEdgePlacedObject.FloorEdgePlacedObjectSave rightEdgeObjectName;*/

        public string upEdgeObjectName;
        public string downEdgeObjectName;
        public string leftEdgeObjectName;
        public string rightEdgeObjectName;
    }

}





