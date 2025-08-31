using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HouseBuildingEdgeGhost : MonoBehaviour
{
    // Start is called before the first frame update

    private FloorEdgeObjectTypeISO floorEdgeObjectTypeISO;
    private Transform visual;
    public event EventHandler OnCanBuild;

    

    public bool canBuild;

    private bool _canBuild
    {
        get => canBuild;
        set
        {
            canBuild = value;
            if ( visual!=null && visual.gameObject != null)
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
        RefreshVisual();
        GridBuildingSystem.Instance.OnSelectedChanged += Instante_OnSelectedChanged;

        OnCanBuild += HandleOnCanBuildChanged;
    }

    // Update is called once per frame

    private void Instante_OnSelectedChanged(object sander, System.EventArgs e)
    {
        RefreshVisual();

    }

    private void LateUpdate()
    {
        FloorEdgePosition floorEdgePosition = GridBuildingSystem.Instance.GetMouseFloorEdgePosition();
        WoodTrimPosition woodTrimPosition=GridBuildingSystem.Instance.GetMouseWoodTriPosition();
        if(floorEdgePosition != null && !floorEdgePosition.isHave)
        {

            transform.position = Vector3.Lerp(transform.position, floorEdgePosition.transform.position, Time.deltaTime * 7.5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, floorEdgePosition.transform.rotation, Time.deltaTime * 12.5f);
            _canBuild = true;
        }

        else if (woodTrimPosition != null && !woodTrimPosition.isHave)
        {
            transform.position = Vector3.Lerp(transform.position, woodTrimPosition.transform.position, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, woodTrimPosition.transform.rotation, Time.deltaTime * 25f);
            _canBuild = true;
        }
        else
        {
            Vector3 targetPosition = GridBuildingSystem.Instance.GetMouseWorldPositionSnap();
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 7.5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 12.5f);
            _canBuild = false;
        }

       // CheckCanBuild();



    }

    private void RefreshVisual()
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        if (GridBuildingSystem.Instance.GetPlaceObjectType() == GridBuildingSystem.PlaceObjectType.EdgeObject)
        {
            FloorEdgeObjectTypeISO floorEdgeObjectTypeISO = GridBuildingSystem.Instance.GetFloorEdgeObjectTypeSO();
            if (floorEdgeObjectTypeISO != null)
            {
                visual = Instantiate(floorEdgeObjectTypeISO.visual.transform, Vector3.zero, Quaternion.identity);
                visual.parent = transform;
                visual.localEulerAngles = Vector3.zero;
                visual.localPosition = Vector3.zero;
               // SetLayerRecursive(visual.gameObject, LayerMask.NameToLayer("Ghost"));

            }
        }
    }

    private void SetLayerRecursive(GameObject targetGameObject, int layer)
    {
        targetGameObject.layer = layer;
        foreach (Transform child in targetGameObject.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }

    
    void ChangeLayerVisual(int layer)
    {
        if (visual != null)
        {
            SetLayerRecursive(visual.gameObject, layer);
        }
    }

    private void HandleOnCanBuildChanged(object sender, EventArgs e)
    {
        if (canBuild)
        {
            Debug.Log("✅ Có thể xây dựng!");
            ChangeLayerVisual(LayerMask.NameToLayer("Ghost"));
        }
        else
        {
            Debug.Log("❌ Không thể xây dựng!");
            ChangeLayerVisual(LayerMask.NameToLayer("Error"));
        }
    }

    private void CheckCanBuild()
    {
        bool newCanBuild=GridBuildingSystem.Instance.CanBuildEdgeObject();
        if (newCanBuild != canBuild)
        {
            canBuild = newCanBuild;
            OnCanBuild?.Invoke(this, EventArgs.Empty);
        }
    }

}


