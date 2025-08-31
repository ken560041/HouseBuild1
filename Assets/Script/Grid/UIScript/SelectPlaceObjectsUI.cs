using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectPlaceObjectsUI : MonoBehaviour
{
    // Start is called before the first frame update
    private Dictionary<PlacedObjectISO, Transform> placedObjectTransformDic;
    private Dictionary<FloorEdgeObjectTypeISO, Transform> floorEdgeObjectTypeTransformDic;


    private Dictionary<Window_DoorObjectTypeISO, Transform> doorObjectTypeTransformDic;
    private Dictionary<Window_DoorObjectTypeISO, Transform> windowsObjectTypeTransformDic;
    private Dictionary<Basic_ObjectTypeISO, Transform> basicObjectTypeTransformDic; // tri
    private Dictionary<RoofObjectTypeISO, Transform> roofObjectTypeTramsformDic;

    private Dictionary<Basic_ObjectTypeISO, Transform> woodStructureTransformDic;

    private Dictionary<LooseObjectISO, Transform> looseObjectTramsformDic;


    private void Awake()
    {
        //Make

        placedObjectTransformDic=new Dictionary<PlacedObjectISO, Transform>();
        floorEdgeObjectTypeTransformDic=new Dictionary<FloorEdgeObjectTypeISO, Transform>();
        windowsObjectTypeTransformDic = new Dictionary<Window_DoorObjectTypeISO, Transform>();
        doorObjectTypeTransformDic = new Dictionary<Window_DoorObjectTypeISO, Transform>();
        basicObjectTypeTransformDic = new Dictionary<Basic_ObjectTypeISO, Transform>();
        roofObjectTypeTramsformDic=new Dictionary<RoofObjectTypeISO, Transform>();
        looseObjectTramsformDic=new Dictionary<LooseObjectISO, Transform>();

        woodStructureTransformDic=new Dictionary<Basic_ObjectTypeISO, Transform>();
        // Nền
        placedObjectTransformDic[HouseBuildingSystemAssets.Instance.woodenFloor1] = transform.Find("ItemSelect/Floor/WoodenFloor1");
        placedObjectTransformDic[HouseBuildingSystemAssets.Instance.woodenFloor_Half2] = transform.Find("ItemSelect/Floor/WoodenFloor_Half2");

        // Cạnh

        
        //floorEdgeObjectTypeTransformDic[HouseBuildingSystemAssets.Instance.Wall3] = transform.Find("ItemSelect/Walls/Wall3_2");
        floorEdgeObjectTypeTransformDic[HouseBuildingSystemAssets.Instance.WoodStairs2] = transform.Find("ItemSelect/Stairs/WoodStairs2");
        floorEdgeObjectTypeTransformDic[HouseBuildingSystemAssets.Instance.WoodenFence_Large] = transform.Find("ItemSelect/Fences/WoodenFence_Large");



        floorEdgeObjectTypeTransformDic[HouseBuildingSystemAssets.Instance.Wall2] = transform.Find("ItemSelect/Walls/Wall2");
        floorEdgeObjectTypeTransformDic[HouseBuildingSystemAssets.Instance.Wall2_Door1] = transform.Find("ItemSelect/Walls/Wall2_Door1");
        floorEdgeObjectTypeTransformDic[HouseBuildingSystemAssets.Instance.Wall2_Window5] = transform.Find("ItemSelect/Walls/Wall2_Window5");
        floorEdgeObjectTypeTransformDic[HouseBuildingSystemAssets.Instance.Wall3_2] = transform.Find("ItemSelect/Walls/Wall3_2");
        floorEdgeObjectTypeTransformDic[HouseBuildingSystemAssets.Instance.Wall3_Door1] = transform.Find("ItemSelect/Walls/Wall3_Door1");
        floorEdgeObjectTypeTransformDic[HouseBuildingSystemAssets.Instance.Wall3_Window2] = transform.Find("ItemSelect/Walls/Wall3_Window2");
        floorEdgeObjectTypeTransformDic[HouseBuildingSystemAssets.Instance.Wall5] = transform.Find("ItemSelect/Walls/Wall5");

        
        



        // Door
        doorObjectTypeTransformDic[HouseBuildingSystemAssets.Instance.door] = transform.Find("ItemSelect/Door/Door");
        windowsObjectTypeTransformDic[HouseBuildingSystemAssets.Instance.window] = transform.Find("ItemSelect/Windows/Window9");


        basicObjectTypeTransformDic[HouseBuildingSystemAssets.Instance.wallWoodTri2_Right] = transform.Find("ItemSelect/Walls/WallWoodTri2_Right");
        basicObjectTypeTransformDic[HouseBuildingSystemAssets.Instance.wallWoodTri2_Left] = transform.Find("ItemSelect/Walls/WallWoodTri2_Left");


        roofObjectTypeTramsformDic[HouseBuildingSystemAssets.Instance.roof1_Piece1] = transform.Find("ItemSelect/Roof/Roof1_Piece1");


        //Loose
        looseObjectTramsformDic[HouseBuildingSystemAssets.Instance.tea]=transform.Find("ItemSelect/Exterior/Bronze_Bottle 1");
        looseObjectTramsformDic[HouseBuildingSystemAssets.Instance.fountain_4_light] = transform.Find("ItemSelect/Exterior/fountain_4_light");
        looseObjectTramsformDic[HouseBuildingSystemAssets.Instance.StonePagoda] = transform.Find("ItemSelect/Exterior/StonePagoda");
        looseObjectTramsformDic[HouseBuildingSystemAssets.Instance.streetbanch1] = transform.Find("ItemSelect/Exterior/streetbanch1");
        looseObjectTramsformDic[HouseBuildingSystemAssets.Instance.well] = transform.Find("ItemSelect/Exterior/well");



        // cạnh đỡ

        woodStructureTransformDic[HouseBuildingSystemAssets.Instance.woodStructure] = transform.Find("ItemSelect/WoodStructure/WoodStructure_Piece3");


        foreach (PlacedObjectISO placedObjectISO in placedObjectTransformDic.Keys)
        {
            placedObjectTransformDic[placedObjectISO].GetComponent<Button_UI>().ClickFunc = () =>
            {
                GridBuildingSystem.Instance.SelectPlacedObjectTypeISO(placedObjectISO);
            };
        }


        foreach(FloorEdgeObjectTypeISO floorEdgeObjectTypeISO in floorEdgeObjectTypeTransformDic.Keys)
        {
            floorEdgeObjectTypeTransformDic[floorEdgeObjectTypeISO].GetComponent<Button_UI>().ClickFunc = () =>
            {

                GridBuildingSystem.Instance.SelectFloorEdgeObjectTypeISO(floorEdgeObjectTypeISO);
            };
        }


        foreach(Window_DoorObjectTypeISO window_DoorObjectTypeISO in doorObjectTypeTransformDic.Keys)
        {
            doorObjectTypeTransformDic[window_DoorObjectTypeISO].GetComponent<Button_UI>().ClickFunc = () =>
            {
                GridBuildingSystem.Instance.SelectWindow_DoorObjectTypeISO(window_DoorObjectTypeISO);
            };
        }

        foreach (Window_DoorObjectTypeISO window_DoorObjectTypeISO in windowsObjectTypeTransformDic.Keys)
        {
            windowsObjectTypeTransformDic[window_DoorObjectTypeISO].GetComponent<Button_UI>().ClickFunc = () =>
            {
                GridBuildingSystem.Instance.SelectWindowObjectTypeISO(window_DoorObjectTypeISO);
            };
        }

        foreach (Basic_ObjectTypeISO basic_ObjectTypeISO in basicObjectTypeTransformDic.Keys)
        {
            basicObjectTypeTransformDic[basic_ObjectTypeISO].GetComponent<Button_UI>().ClickFunc = () =>
            {
                GridBuildingSystem.Instance.SelectTri_WoodenObjectTypeISO(basic_ObjectTypeISO);
            };
        }

        foreach (RoofObjectTypeISO roofObjectTypeISO in roofObjectTypeTramsformDic.Keys)
        {
            roofObjectTypeTramsformDic[roofObjectTypeISO].GetComponent<Button_UI>().ClickFunc = () =>
            {
                GridBuildingSystem.Instance.SelectRoofObjectTypeISO(roofObjectTypeISO);
            };
        }


        foreach(Basic_ObjectTypeISO basic_ObjectTypeISO in woodStructureTransformDic.Keys)
        {
            woodStructureTransformDic[basic_ObjectTypeISO].GetComponent<Button_UI>().ClickFunc = () =>
            {
                GridBuildingSystem.Instance.SelectWoodStructureObjectTypeISO(basic_ObjectTypeISO);
            };
        }

        //Loose
        foreach(LooseObjectISO looseObjectISO in looseObjectTramsformDic.Keys)
        {
            looseObjectTramsformDic[looseObjectISO].GetComponent<Button_UI>().ClickFunc = () =>
            {
                GridBuildingSystem.Instance.SelectLooseObjectTypeISO(looseObjectISO);
            };
        }



    }
}
