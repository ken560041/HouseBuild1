using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBuildingWoodTriGhost : MonoBehaviour
{

    // Start is called before the first frame update
    Transform visual;

    public bool canBuild;

    private bool _canBuild
    {
        get => canBuild;
        set
        {
            canBuild = value;
            if (visual!=null && visual.gameObject != null)
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




    void Start()
    {
        GridBuildingSystem.Instance.OnSelectedChanged += Instante_OnSelectedChanged;

    }

    // Update is called once per frame
    void  LateUpdate()
    {
        WoodTrimPosition woodTrimPosition= GridBuildingSystem.Instance.GetMouseWoodTriPosition();
        if (woodTrimPosition != null && !woodTrimPosition.isHave)
        {
            transform.position=Vector3.Lerp(transform.position, woodTrimPosition.transform.position, Time.deltaTime* 15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, woodTrimPosition.transform.rotation, Time.deltaTime * 25f);
            _canBuild = true;
        }
        else
        {
            Vector3 targetPosition = GridBuildingSystem.Instance.GetMouseWorldPositionSnap();
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 25f);
            _canBuild= false;
        }



    }
    private void Instante_OnSelectedChanged(object sander, System.EventArgs eventArgs)
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
        if (GridBuildingSystem.Instance.GetPlaceObjectType() == GridBuildingSystem.PlaceObjectType.WoodTriObject)
        {
            Basic_ObjectTypeISO basic_ObjectTypeISO = GridBuildingSystem.Instance.GetBasic_ObjectTypeISO();
            if (basic_ObjectTypeISO != null)
            {
                visual = Instantiate(basic_ObjectTypeISO.visual.transform, Vector3.zero, Quaternion.identity);
                visual.parent = transform;
                visual.localEulerAngles = Vector3.zero;
                visual.localPosition = Vector3.zero;
            }
        }


    }



}
