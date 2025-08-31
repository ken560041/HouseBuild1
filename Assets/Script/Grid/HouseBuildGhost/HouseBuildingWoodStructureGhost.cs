using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBuildingWoodStructureGhost : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform visual;
    void Start()
    {
        RefreshVisual();
        GridBuildingSystem.Instance.OnSelectedChanged += Instante_OnSelectedChanged;

       
    }

    // Update is called once per frame

    private void Instante_OnSelectedChanged(object sander, System.EventArgs e)
    {
        RefreshVisual();

    }
    // Update is called once per frame
    void  LateUpdate()
    {
        AnglePosition anglePosition= GridBuildingSystem.Instance.GetMouseAnglePosition();

        if(anglePosition != null&& !anglePosition.isHave)
        {
            transform.position = Vector3.Lerp(transform.position, anglePosition.transform.position, Time.deltaTime *15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, anglePosition.transform.rotation, Time.deltaTime * 25f);
        }
        else
        {
            Vector3 targetPosition=GridBuildingSystem.Instance.GetMouseWorldPositionSnap();
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 25f);
        }
    }

    private void RefreshVisual()
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        if (GridBuildingSystem.Instance.GetPlaceObjectType() == GridBuildingSystem.PlaceObjectType.WoodStructureObject)
        {
            Basic_ObjectTypeISO basic_ObjectTypeISO = GridBuildingSystem.Instance.GetBasic_ObjectTypeISO();
            if (basic_ObjectTypeISO != null)
            {
                visual = Instantiate(basic_ObjectTypeISO.visual.transform, Vector3.zero, Quaternion.identity);
                visual.parent = transform;
                visual.localEulerAngles = Vector3.zero;
                visual.localPosition = Vector3.zero;
                // SetLayerRecursive(visual.gameObject, LayerMask.NameToLayer("Ghost"));

            }
        }
    }

}
