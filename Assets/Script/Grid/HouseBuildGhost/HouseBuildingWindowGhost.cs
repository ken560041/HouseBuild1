using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBuildingWindowGhost : MonoBehaviour
{
    // Start is called before the first frame update
 
    // Start is called before the first frame update
    private Transform visual;


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

        
        DefaultPosition defaultPosition = GridBuildingSystem.Instance.GetMouseWindowPosition();



        if (defaultPosition != null && defaultPosition.isHave!=true)
        {
            transform.position = Vector3.Lerp(transform.position, defaultPosition.transform.position, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, defaultPosition.transform.rotation, Time.deltaTime * 25f);
        }

        else
        {
            Vector3 targetPosition = GridBuildingSystem.Instance.GetMouseWorldPositionSnap();
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 25f);
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
        if (GridBuildingSystem.Instance.GetPlaceObjectType() == GridBuildingSystem.PlaceObjectType.WindowObject)
        {
            Window_DoorObjectTypeISO window_DoorObjectTypeISO = GridBuildingSystem.Instance.GetWindow_DoorObjectTypeSO();
            if (window_DoorObjectTypeISO != null)
            {
                visual = Instantiate(window_DoorObjectTypeISO.visual.transform, Vector3.zero, Quaternion.identity);
                visual.parent = transform;
                visual.localEulerAngles = Vector3.zero;
                visual.localPosition = Vector3.zero;
                // SetLayerRecursive(visual.gameObject, LayerMask.NameToLayer("Ghost"));

            }
        }

    }


}

