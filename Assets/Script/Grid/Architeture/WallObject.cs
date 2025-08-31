using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FloorEdgePlacedObject;

public class WallObject : FloorEdgePlacedObject
{
    // Start is called before the first frame update
    private DoorObject doorObject;
    [SerializeField] private DoorPosition doorPosition;
    public GameObject doorHinge;
    public void PlaceDoor(DoorPosition doorPosition, Window_DoorObjectTypeISO window_DoorObjectTypeISO)
    {
       // this.doorPosition = doorPosition;

        if (doorPosition == null)
        {
            Debug.LogError("PlaceDoor failed: doorPosition is null");
            return;
        }

        if (window_DoorObjectTypeISO == null)
        {
            Debug.LogError("PlaceDoor failed: window_DoorObjectTypeISO is null");
            return;
        }

        if (window_DoorObjectTypeISO.prefab == null)
        {
            Debug.LogError("PlaceDoor failed: window_DoorObjectTypeISO.prefab is null");
            return;
        }


        Transform doorObjectTransform = Instantiate(window_DoorObjectTypeISO.prefab.transform, doorPosition.transform.position, doorPosition.transform.rotation);
        this.doorPosition.isHave=true;

        DoorObject newDoorObject = doorObjectTransform.GetComponent<DoorObject>();
        if (newDoorObject != null)
        {
            newDoorObject.onDestroyed += HandleDoorDestroyed;
            doorObject = newDoorObject;
        }

        if (doorHinge != null)
        {
            Rigidbody connectedRb = doorHinge.GetComponent<Rigidbody>();

            // Kiểm tra xem đối tượng có Rigidbody không
            if (connectedRb == null)
            {
                connectedRb = doorHinge.AddComponent<Rigidbody>();
                connectedRb.isKinematic = true;  // Đảm bảo đối tượng không chịu tác động của lực
            }

           newDoorObject.hingeJoint.connectedBody = connectedRb;
            Rigidbody rb = newDoorObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;

        }
    }

    public  WallObjectSave Save()
    {
        FloorEdgePlacedObjectSave baseSave = base.Save();
        WallObjectSave wallSave = new WallObjectSave
        {
            floorEdgePlacedObjectISOName = baseSave.floorEdgePlacedObjectISOName,
            data = baseSave.data,
            doorTypeISOName = doorObject != null ? doorObject.GetWindow_DoorObjectTypeIsoName() : "",
            objectTypeName ="WallObject",
        };

        // Lưu thêm thông tin cửa (nếu có)
        

        return wallSave;
    }
    public void Load(string data)
    {
       this.PlaceDoor(this.doorPosition, HouseBuildingSystemAssets.Instance.GetWindow_DoorObjectTypeISOFromName(data));
    }


    public override void DestroyMySelf()
    {
        base.DestroyMySelf();

        if(doorObject != null)
        {
            Destroy(doorObject.gameObject);
        }
    }


    private void HandleDoorDestroyed(DoorObject destroyedDoor)
    {
        doorPosition.isHave = false;
    }



    [System.Serializable]
    public class WallObjectSave : FloorEdgePlacedObjectSave
{
        public bool hasDoor;
        public string doorTypeISOName; // tên hoặc ID cửa (lấy từ Window_DoorObjectTypeISO)
        
    }

}
