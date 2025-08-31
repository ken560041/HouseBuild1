using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GridBuildingSystem;

public class PlacedObject : MonoBehaviour
{
    // Start is called before the first frame update

    public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectISO.Dir dir, PlacedObjectISO placedObjectISO, int gridLevel)
    {
       
        Transform placedObjectTransform = Instantiate(placedObjectISO.prefab.transform, worldPosition ,
            Quaternion.Euler(0, placedObjectISO.GetRotationAngle(dir), 0));
        
        PlacedObject placedObject= placedObjectTransform.GetComponent<PlacedObject>();
        placedObject.placedObjectISO = placedObjectISO;
        placedObject.origin=origin;
        placedObject.dir=dir;
        placedObject.gridLevel=gridLevel;


        return placedObject;
    }


    private PlacedObjectISO placedObjectISO;
    private Vector2Int origin;
    private PlacedObjectISO.Dir dir;
    private int gridLevel;


    public List<Vector2Int> GetGridPositionList()
    {
        return placedObjectISO.GetGridPositionList( origin,dir);
    }

    public Vector2Int GetOrigin()
    {
        return origin;
    }

    public int GetGridLevel()
    {
        return gridLevel;
    }

    public virtual void DestroyMySelf()
    {
        Destroy(gameObject);
    }

    
    public SaveObject GetSaveObject()
    {
        return new SaveObject{
            placedObjectISOName = placedObjectISO.name,
            origin = origin,
            dir = dir,
            floorPlacedObjectSave = (this is FloorPlacedObject) ? ((FloorPlacedObject)this).Save() : "",
        };
    }


    [System.Serializable]
    public class SaveObject
    {
        public string placedObjectISOName;
        public Vector2Int origin;
        public PlacedObjectISO.Dir dir;
        public string floorPlacedObjectSave;
    }
    
}
