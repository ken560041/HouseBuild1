using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FloorEdgePlacedObject;

public class RoofObject :MonoBehaviour
{
    // Start is called before the first frame update

    public enum Edge
    {
     Left, Right
    }

    [SerializeField] private Basic_ObjectTypeISO basic_ObjectType;
    public Edge pivot;

   [SerializeField] private RoofTrussPosition nextRoofTrussPosition;
   [SerializeField] private RoofTrussPosition prevRoofTrussPosition;

    private RoofObject nextRoofObject;
    private RoofObject prevRoofObject;

    

    private WoodTriObject nextWoodTriObject;
    private WoodTriObject prevWoodTriObject;

    public Action<RoofObject> onDestroyed;

    private void OnDestroy()
    {
        onDestroyed?.Invoke(this);
    }


    public string GetRoofBasicObjectTypeISOName()
    {
        return basic_ObjectType.name;
    } 


    public static RoofObject Create(RoofTrussPosition roofTrussPosition, RoofObjectTypeISO roofObjectTypeISO, Edge pivot )
    {
        if (roofObjectTypeISO == null)
        {
            Debug.LogError("basic_ObjectTypeISO is null!");
            return null;
        }
        if (roofTrussPosition == null)
        {
            Debug.LogError("roofTrussPosition is null!");
            return null;
        }

        GameObject prefabToUse = null;

        if(pivot==Edge.Right && roofObjectTypeISO.right != null)
        {
            prefabToUse = roofObjectTypeISO.right.prefab;
        }
        else if (pivot == Edge.Left && roofObjectTypeISO.left != null)
        {
            prefabToUse = roofObjectTypeISO.left.prefab;
        }
        if (prefabToUse == null)
        {
            Debug.LogError("Prefab to use is null based on pivot!");
            return null;
        }





        Transform newTransform = Instantiate(prefabToUse.transform, roofTrussPosition.transform.position, roofTrussPosition.transform.rotation);
       // roofTrussPosition.isHave = true;
        RoofObject roofObject = newTransform.GetComponent<RoofObject>();
        return roofObject;

    }

    public static RoofObject Create(RoofTrussPosition roofTrussPosition, Basic_ObjectTypeISO basic_ObjectTypeISO)
    {
        if (roofTrussPosition == null)
        {
            Debug.LogError("roofTrussPosition is NULL in RoofObject.Create");
            return null;
        }

        if (basic_ObjectTypeISO == null)
        {
            Debug.LogError("basic_ObjectTypeISO is NULL in RoofObject.Create"); 
            return null;
        }

        if (basic_ObjectTypeISO.prefab == null)
        {
            Debug.LogError("basic_ObjectTypeISO.prefab is NULL in RoofObject.Create");
            return null;
        }


        Transform newTransform = Instantiate(basic_ObjectTypeISO.prefab.transform, roofTrussPosition.transform.position, roofTrussPosition.transform.rotation);
        RoofObject roofObject = newTransform.GetComponent<RoofObject>();
        if (roofObject == null)
        {
            Debug.LogError("Prefab does not have RoofObject component!");
        }
        return roofObject;
    }


    public void PlaceRoofObject(RoofTrussPosition roofTrussPosition, RoofObjectTypeISO roofObjectTypeISO)
    {
        Transform newRoofObjectTransform;

        //RoofObject testroofObject=roofTrussPosition.transform.parent.GetComponent<RoofObject>();
        
        RoofTrussPosition newRoofTrussPosition = GetRoofTrussPosition(roofTrussPosition.edge);
        RoofObject currentObject = GetRoofObject(roofTrussPosition.edge);
        if (currentObject != null)
        {
            Debug.Log(roofTrussPosition.edge.ToString() + currentObject.ToString());

            return;
        }

        if (this.pivot == Edge.Right&& roofTrussPosition.isHave==false)
        {
            newRoofObjectTransform = Instantiate(roofObjectTypeISO.right.prefab.transform, newRoofTrussPosition.transform.position, newRoofTrussPosition.transform.rotation);

        }
        else if(this.pivot == Edge.Left && roofTrussPosition.isHave == false)
        {
            newRoofObjectTransform = Instantiate(roofObjectTypeISO.left.prefab.transform, newRoofTrussPosition.transform.position, newRoofTrussPosition.transform.rotation);
        }
        else
        {
            return;
        }

        
        RoofObject newRoofObject=newRoofObjectTransform.GetComponent<RoofObject>();
        if(newRoofObject != null )
        {
            SetRoofObject(newRoofObject, roofTrussPosition.edge);
            roofTrussPosition.isHave=true;
        }
    }


    public void PlaceRoofObject(RoofTrussPosition roofTrussPosition, RoofObjectTypeISO roofObjectTypeISO, out RoofObject roofObject)
    {
        roofObject = null;
        RoofTrussPosition newRoofTrussPosition = GetRoofTrussPosition(roofTrussPosition.edge);
        RoofObject currentObject = GetRoofObject(roofTrussPosition.edge);
        if (currentObject != null)
        {
            Debug.Log(roofTrussPosition.edge.ToString() + currentObject.ToString());


        }

        if (roofTrussPosition.isHave == false)
        {
            roofObject = Create(newRoofTrussPosition, roofObjectTypeISO, this.pivot);
            if (roofObject != null)
            {
                SetRoofObject(roofObject, roofTrussPosition.edge);
                roofTrussPosition.isHave = true;
            }
        }


    }

    public void PlaceRoofObject(RoofTrussPosition roofTrussPosition, Basic_ObjectTypeISO basic_ObjectTypeISO, out RoofObject roofObject)
    {
        roofObject = null;
        RoofTrussPosition newRoofTrussPosition = GetRoofTrussPosition(roofTrussPosition.edge);
        RoofObject currentObject = GetRoofObject(roofTrussPosition.edge);
        if (currentObject != null)
        {
            Debug.Log(roofTrussPosition.edge.ToString() + currentObject.ToString());


        }

        if (roofTrussPosition.isHave == false)
        {
            roofObject = Create(newRoofTrussPosition, basic_ObjectTypeISO);
            if (roofObject != null)
            {
                SetRoofObject(roofObject, roofTrussPosition.edge);
                roofTrussPosition.isHave = true;
            }
        }


    }






    private RoofTrussPosition GetRoofTrussPosition(Edge edge)
    {

        if (edge == pivot)
        {
            return prevRoofTrussPosition;
        }
        else
        {
            return nextRoofTrussPosition;
        }

        
    }
public void SetPrevRoofObject(RoofObject roofObject)
{
    this.prevRoofObject = roofObject;
}


private RoofObject GetRoofObject(Edge edge)
    {
        if (edge == pivot)
        {
            return prevRoofObject;
        }
        else
        {
            return nextRoofObject;
        }
    }

    private void SetRoofObject(RoofObject roofObject, Edge edge)
    {
        nextRoofObject = roofObject;
        roofObject.prevRoofObject = this;
        roofObject.prevRoofTrussPosition.isHave = true;


    }

    public void SetPrevTrussIsHave(bool value)
    {
        if (prevRoofTrussPosition != null)
        {
            prevRoofTrussPosition.isHave = value;
        }
    }
    public void SetNextTrussIsHave(bool value)
    {
        if (nextRoofTrussPosition != null)
        {
            nextRoofTrussPosition.isHave = value;
        }
    }

 




    public void DestroyMySelf()
    {
        if (prevRoofObject != null)
        {
            prevRoofObject.nextRoofTrussPosition.isHave = false;
            prevRoofObject.nextRoofObject = null;
        }
        Destroy(this.gameObject);

        if (nextRoofObject != null)
        {
            nextRoofObject.DestroyMySelf();
        }
    }


    public string SaveData()
    {
        return JsonUtility.ToJson(new RoofObjectData
         {
             nextRoofObjectName = null,
             prevRoofObjectName = null,
         });
    }

    public RoofObjectSave Save()
    {
        return new RoofObjectSave
        {

            roofObjectISOName = GetRoofBasicObjectTypeISOName(),
            next = nextRoofObject != null ? JsonUtility.ToJson (nextRoofObject.Save()) : null,
            
            prev = null

        };
        
    }

    /*public void Load(string saveString)
    {
        RoofObjectData roofObjectData = JsonUtility.FromJson<RoofObjectData>(saveString);
        if (!string.IsNullOrEmpty(roofObjectData.nextRoofObjectName))
        {
            var roofIso=HouseBuildingSystemAssets.Instance.GetBasic_ObjectTypeISOFromName(roofObjectData.nextRoofObjectName);
            PlaceRoofObject(nextRoofTrussPosition, roofIso, nextRoofObject);
        }
    }*/


    public void Load(string data)
    {
        RoofObjectSave roofObjectSave = JsonUtility.FromJson<RoofObjectSave>(data);
        if (roofObjectSave.roofObjectISOName != null)
        {
            PlaceRoofObject(this.nextRoofTrussPosition, HouseBuildingSystemAssets.Instance.GetBasic_ObjectTypeISOFromName(roofObjectSave.roofObjectISOName), out RoofObject roofObject);
            if (!string.IsNullOrEmpty(roofObjectSave.next))
            {
                roofObject.Load(roofObjectSave.next);
            }
        }

    }




    [System.Serializable]
    public class RoofObjectSave
    {
        public string roofObjectISOName;
        public string next;
        public string prev;
    }


    public class RoofObjectData
    {
        public string nextRoofObjectName;
        public string prevRoofObjectName;
    }
}
