using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RoofObject;

public class WoodTriObject : MonoBehaviour
{
    // Start is called before the first frame update
    public Basic_ObjectTypeISO basic_ObjectTypeISO;

    [SerializeField] private RoofTrussPosition roofTrussPosition;
    private RoofObject roofObject;

    public Action<WoodTriObject> onDestroyed;
    private void OnDestroy()
    {
        onDestroyed?.Invoke(this);
    }


    public static WoodTriObject Create(WoodTrimPosition woodTrimPosition, Basic_ObjectTypeISO basic_ObjectTypeISO)
    {
        if (basic_ObjectTypeISO == null) {
            Debug.LogError("basic_ObjectTypeISO is null!");
            return null;
        }
        if (basic_ObjectTypeISO.prefab == null)
        {
            Debug.LogError("basic_ObjectTypeISO.prefab is null!");
            return null;
        }
        if (woodTrimPosition == null)
        {
            Debug.LogError("woodTrimPosition is null!");
            return null;
        }
        Transform woodTriTransform = Instantiate(basic_ObjectTypeISO.prefab.transform, woodTrimPosition.transform.position, woodTrimPosition.transform.rotation);

        WoodTriObject newWoodTriObject = woodTriTransform.GetComponent<WoodTriObject>();
        return newWoodTriObject;
    }

    public string GetWoodTriuObjectTypeIsoName()
    {
        return basic_ObjectTypeISO.name;
    }
        

    public void PlaceRoofObject(RoofTrussPosition roofTrussPosition, RoofObjectTypeISO roofObjectTypeISO)
    {
        Transform newRoofObjectTransform;
        RoofTrussPosition newRoofTrussTransform = GetRoofTrussPosition();

        if (GetRoofObject() != null)
        {
            return;
        }

        if (roofTrussPosition.edge == RoofObject.Edge.Right&& newRoofTrussTransform.isHave==false)
        {
            newRoofObjectTransform = Instantiate(roofObjectTypeISO.right.prefab.transform, newRoofTrussTransform.transform.position, newRoofTrussTransform.transform.rotation);

            Debug.Log("place trai");
        }

        else if(roofTrussPosition.edge == RoofObject.Edge.Left && newRoofTrussTransform.isHave == false)
        {
            newRoofObjectTransform = Instantiate(roofObjectTypeISO.left.prefab.transform, newRoofTrussTransform.transform.position, newRoofTrussTransform.transform.rotation);
        }

        else
        {
            newRoofObjectTransform = Instantiate(roofObjectTypeISO.left.prefab.transform, newRoofTrussTransform.transform.position, newRoofTrussTransform.transform.rotation);
        }

        RoofObject newRoofObject = newRoofObjectTransform.GetComponent<RoofObject>();

        if(newRoofObject!= null )
        {
            SetRoofObject(newRoofObject);

        }
    }

    public void PlaceRoofObject(RoofTrussPosition roofTrussPosition, Basic_ObjectTypeISO basic_ObjectTypeISO, out RoofObject roofObject)
    {
        roofObject = null;
        roofObject = RoofObject.Create(roofTrussPosition, basic_ObjectTypeISO);
        
        if (roofObject != null)
        {
            SetRoofObject(roofObject);

        }
    }


    public void DestroyMySelf()
    {
        if(roofObject!= null)
        {
            roofObject.DestroyMySelf();
        }
        Destroy(this.gameObject);
    }

    private void HandleRoofDestroyed(RoofObject obj)
    {
        roofTrussPosition.isHave = false;
    }

    private RoofTrussPosition GetRoofTrussPosition() {

        return roofTrussPosition;
    }

    private RoofObject GetRoofObject()
    {
        return roofObject;
    }

    private void SetRoofObject(RoofObject roofObject)
    {
        roofObject.SetPrevTrussIsHave(true);
        roofObject.onDestroyed += HandleRoofDestroyed;
        this.roofObject = roofObject;
        
        
        this.roofTrussPosition.isHave = true;
        

    }


    public string SaveData()
    {
        return JsonUtility.ToJson(new WoodTriObjectDataSave { 
        
            roofObjectName=roofObject?.Save(),
        });
    }

    public WoodTriObjectSave Save()
    {
        return new WoodTriObjectSave
        {
            woodTriObjectName = GetWoodTriuObjectTypeIsoName(),
            data = this.SaveData()
        };
    }


    public void Load(string saveString)
    {
        Debug.Log(saveString);
        WoodTriObjectDataSave data = JsonUtility.FromJson<WoodTriObjectDataSave>(saveString);
        RoofObjectSave roofObjectSave = data.roofObjectName;
        string temp1=JsonUtility.ToJson(roofObjectSave);
        roofObjectSave = JsonUtility.FromJson<RoofObjectSave>(temp1);

        string roofObjectISOName = roofObjectSave.roofObjectISOName;
        Debug.Log(roofObjectISOName);

        PlaceRoofObject(roofTrussPosition, HouseBuildingSystemAssets.Instance.GetBasic_ObjectTypeISOFromName(roofObjectISOName), out RoofObject roofObject);
        string nextString = roofObjectSave.next;
        Debug.Log(nextString);
        if (!string.IsNullOrEmpty(nextString))
        {
            roofObject.Load(nextString);
        }
    }


    [System.Serializable]

    public class WoodTriObjectSave
    {
        public string woodTriObjectName;
        public string data;
    }

    [System.Serializable]
    public class WoodTriObjectDataSave
    {
        public RoofObjectSave roofObjectName;
    }

 }

