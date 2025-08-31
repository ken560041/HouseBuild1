using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBuildingDoorGhost : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform visual;


    public bool canBuild;

    private bool _canBuild
    {
        get => canBuild;
        set
        {
            canBuild = value;
            if (visual != null&& visual.gameObject!=null)
            {
                if (canBuild)
                {
                    visual.gameObject.layer = LayerMask.NameToLayer("Ghost");
                }
                else
                {
                    visual.gameObject.layer = LayerMask.NameToLayer("Default");
                }
            }
        }
    }

    public void Start()
    {
        RefreshVisual();
        GridBuildingSystem.Instance.OnSelectedChanged += Instante_OnSelectedChanged;
    }


    private void LateUpdate()
    {
        /// Update transform
        /// 
        /// 

        DoorPosition doorPosition= GridBuildingSystem.Instance.GetMouseDoorPosition();
        DefaultPosition defaultPosition=GridBuildingSystem.Instance.GetMouseWindowPosition();

        if(doorPosition != null && !doorPosition.isHave)
        {
            transform.position = Vector3.Lerp(transform.position, doorPosition.transform.position, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, doorPosition.transform.rotation, Time.deltaTime * 25f);

            _canBuild = true;
        }

        

        else
        {
            Vector3 targetPosition = GridBuildingSystem.Instance.GetMouseWorldPositionSnap();
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 25f);
            _canBuild = false;
        }





    }


    private void Instante_OnSelectedChanged(object sander, System.EventArgs e)
    {
        RefreshVisual();

    }

    private void RefreshVisual()
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        if (GridBuildingSystem.Instance.GetPlaceObjectType() == GridBuildingSystem.PlaceObjectType.DoorObject)
        {
            Window_DoorObjectTypeISO window_DoorObjectTypeISO = GridBuildingSystem.Instance.GetWindow_DoorObjectTypeSO(); 
            if (window_DoorObjectTypeISO != null)
            {
                visual = Instantiate(window_DoorObjectTypeISO.visual.transform, Vector3.zero, Quaternion.identity);
                visual.parent = transform;
                visual.localEulerAngles = Vector3.zero;
                visual.localPosition = Vector3.zero;
            }
        }

       

    }


}
