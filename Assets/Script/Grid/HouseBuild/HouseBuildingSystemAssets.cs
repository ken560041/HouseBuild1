using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBuildingSystemAssets : MonoBehaviour
{
    // Start is called before the first frame update
    
    public static HouseBuildingSystemAssets Instance { get; private set; }


    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }   
    }

    public PlacedObjectISO[] placedObjectISOArray;

    public PlacedObjectISO woodenFloor1;
    public PlacedObjectISO woodenFloor_Half2;
    public PlacedObjectISO woodenFloor_Small;

    public FloorEdgeObjectTypeISO[] floorEdgeObjectTypeISOArray;


    //Tường
    public FloorEdgeObjectTypeISO Wall2;
    public FloorEdgeObjectTypeISO Wall2_Door1;
    public FloorEdgeObjectTypeISO Wall2_Window5;
    public FloorEdgeObjectTypeISO Wall3_2;
    public FloorEdgeObjectTypeISO Wall3_Door1;
    public FloorEdgeObjectTypeISO Wall3_Window2;
    public FloorEdgeObjectTypeISO Wall5;



    // ngoai le
    public FloorEdgeObjectTypeISO WoodStairs2;
    public FloorEdgeObjectTypeISO WoodenFence_Large;

    
    
    
    



    public Window_DoorObjectTypeISO[] window_DoorObjectTypeISOArray;
    public Window_DoorObjectTypeISO door;
    public Window_DoorObjectTypeISO window;

    public Basic_ObjectTypeISO[] basic_ObjectTypeISOArray;
    public Basic_ObjectTypeISO wallWoodTri2_Left;
    public Basic_ObjectTypeISO wallWoodTri2_Right;

    public RoofObjectTypeISO roof1_Piece1;


    public Basic_ObjectTypeISO woodStructure;


    public LooseObjectISO[] looseObjectISOArray;
    public LooseObjectISO tea;
    public LooseObjectISO fountain_4_light;
    public LooseObjectISO StonePagoda;
    public LooseObjectISO streetbanch1;
    public LooseObjectISO well;

    public PlacedObjectISO GetPlacedObjectISOFromName(string placedObjectISOName)
    {
        foreach (PlacedObjectISO placedObjectISO in placedObjectISOArray)
        {
            if (placedObjectISO.name == placedObjectISOName)
            {
                return placedObjectISO;
            }
        }
        return null;
    }

    public FloorEdgeObjectTypeISO GetFloorEdgeObjectTypeISOFromName(string floorEdgeObjectTypeISOName) {

        foreach (FloorEdgeObjectTypeISO floorEdgeObjectTypeISO in floorEdgeObjectTypeISOArray)
        {
            if(floorEdgeObjectTypeISO.name == floorEdgeObjectTypeISOName)
            {
                return floorEdgeObjectTypeISO;
            }
        }
        return null;
    }


    public Window_DoorObjectTypeISO GetWindow_DoorObjectTypeISOFromName(string window_doorObjectTypeISOName)
    {
        foreach(Window_DoorObjectTypeISO window_ in window_DoorObjectTypeISOArray)
        {
            if(window_.name== window_doorObjectTypeISOName)
            {
                return window_;
            }
        }
        return null;
    }


    public LooseObjectISO GetLooseObjectISOFromName(string looseObjectISOName)
    {
        foreach(LooseObjectISO looseObjectISO in looseObjectISOArray)
        {
            if(looseObjectISO.name == looseObjectISOName)
            {
                return looseObjectISO;
            }
        }

        return null;
    }

    public Basic_ObjectTypeISO GetBasic_ObjectTypeISOFromName(string basic_objectISOName)
    {
        foreach(Basic_ObjectTypeISO basic_ in basic_ObjectTypeISOArray)
        {
            if(basic_.name == basic_objectISOName) { return basic_; }
        }
        return null;
    }

}
