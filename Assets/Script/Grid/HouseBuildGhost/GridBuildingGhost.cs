using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlacedObjectISO;
//using static UnityEditor.Experimental.GraphView.GraphView;



public class GridBuildingGhost : MonoBehaviour
{
    // Start is called before the first frame update\


    public event EventHandler OnCanBuild;
    /// <summary>
    /// 
    /// </summary>
    private Transform visual;

    private bool canBuild;

    public bool _canBuildTest;

    private bool canBuildTest
    {
        get => _canBuildTest;
        set
        {
            _canBuildTest = value;

            if (visual != null)
            {
                if (_canBuildTest)
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

    private void checkCanBuildTest()
    {
        PlacedObjectISO placedObjectTypeSO = GridBuildingSystem.Instance.GetPlacedObjectTypeSO();
        PlacedObjectISO.Dir dir = GridBuildingSystem.Instance.GetDir();

        Vector3 tempTransform = GridBuildingSystem.Instance.GetMGetMouseWorldPositionNoSnap();

        GridBuildingSystem.Instance.selectGrid.GetXZ(tempTransform, out int x, out int y);
        Vector2Int placedObjectOrigin = new Vector2Int(x, y);

        
        bool newCanBuild = GridBuildingSystem.Instance.CanBuildFloor(
            GridBuildingSystem.Instance.selectGrid,
            placedObjectOrigin,
            placedObjectTypeSO,
            dir
        );
        canBuildTest = newCanBuild;


    }



    
    private void Start()
    {
        
        RefreshVisual();
        OnCanBuild += HandleOnCanBuildChanged;
        GridBuildingSystem.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
    }

    private void Instance_OnSelectedChanged(object sender, System.EventArgs e)
    {
        RefreshVisual();
    }


    private void Instance_OnCanBuild(object sender,System.EventArgs e)
    {
        
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = GridBuildingSystem.Instance.GetMouseWorldPositionSnap();
        //targetPosition.y += .1f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);

        transform.rotation = Quaternion.Lerp(transform.rotation, GridBuildingSystem.Instance.GetPlacedObjectRotation(), Time.deltaTime * 25f);
        PlacedObjectISO placedObjectTypeSO = GridBuildingSystem.Instance.GetPlacedObjectTypeSO();
        if (placedObjectTypeSO != null && GridBuildingSystem.Instance.GetPlaceObjectType() == GridBuildingSystem.PlaceObjectType.GridObject)
        {
            // CheckCanBuild();

            checkCanBuildTest();
        }

    }

    private void Update()
    {
       /* PlacedObjectISO placedObjectTypeSO = GridBuildingSystem.Instance.GetPlacedObjectTypeSO();
        if (placedObjectTypeSO != null&& GridBuildingSystem.Instance.GetPlaceObjectType()==GridBuildingSystem.PlaceObjectType.GridObject)
        {
           // CheckCanBuild();

            checkCanBuildTest();
        }*/
    }

    void RefreshVisual()
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        
        if (GridBuildingSystem.Instance.GetPlaceObjectType() == GridBuildingSystem.PlaceObjectType.GridObject)
        {
            PlacedObjectISO placedObjectTypeSO = GridBuildingSystem.Instance.GetPlacedObjectTypeSO();
            if (placedObjectTypeSO != null)
            {
                visual = Instantiate(placedObjectTypeSO.visual.transform, Vector3.zero, Quaternion.identity);
                visual.parent = transform;
                visual.localPosition = Vector3.zero;
                visual.localEulerAngles = Vector3.zero;
                
               // Debug.Log(visual.gameObject.layer);

                
            }
            else
            {
                return;
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
        if(visual != null)
        {
            SetLayerRecursive(visual.gameObject, layer);
        }
    }


    

    private void CheckCanBuild()
    {
        if (GridBuildingSystem.Instance == null)
        {
            Debug.LogError("GridBuildingSystem.Instance is null!");
            return;
        }

        if (GridBuildingSystem.Instance.selectGrid == null)
        {
            Debug.LogError("selectGrid is null!");
            return;
        }

        PlacedObjectISO placedObjectTypeSO = GridBuildingSystem.Instance.GetPlacedObjectTypeSO();
        if (placedObjectTypeSO == null)
        {
            Debug.LogError("placedObjectTypeSO is null!");
            return;
        }

        PlacedObjectISO.Dir dir = GridBuildingSystem.Instance.GetDir();
        if (dir == null)
        {
            Debug.LogError("Direction is null!");
            return;
        }

        GridBuildingSystem.Instance.selectGrid.GetXZ(transform.position, out int x, out int y);
        Vector2Int placedObjectOrigin = new Vector2Int(x, y);

        bool newCanBuild = GridBuildingSystem.Instance.CanBuildFloor(
            GridBuildingSystem.Instance.selectGrid,
            placedObjectOrigin,
            placedObjectTypeSO,
            dir
        );

        if (newCanBuild != canBuild)
        {
            canBuild = newCanBuild;
            OnCanBuild?.Invoke(this, EventArgs.Empty);
            Debug.Log("Can Build Status Changed: " + canBuild);
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


}
