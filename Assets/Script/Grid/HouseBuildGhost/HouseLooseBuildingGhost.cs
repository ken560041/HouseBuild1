using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseLooseBuildingGhost : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform visual;
    private void Start()
    {
        RefreshVisual();
        GridBuildingSystem.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
    }

    private void Instance_OnSelectedChanged(object sender, System.EventArgs e)
    {
        RefreshVisual();
    }

    private void LateUpdate()
    {
         Vector3 target=GridBuildingSystem.Instance.GetMouseWorldPosition();

        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 25f);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,GridBuildingSystem.Instance.GetLooseObjectEulerY(),0), Time.deltaTime * 15f);

    }

    private void RefreshVisual()
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        if (GridBuildingSystem.Instance.GetPlaceObjectType() == GridBuildingSystem.PlaceObjectType.LooseObject)
        {
            LooseObjectISO looseObjectSO = GridBuildingSystem.Instance.GetLooseObjectSO();

            if (looseObjectSO != null)
            {
                visual = Instantiate(looseObjectSO.visual.transform, Vector3.zero, Quaternion.identity);
                visual.parent = transform;
                visual.localPosition = Vector3.zero;
                visual.localEulerAngles = Vector3.zero;
                

            }
        }
    }

}
