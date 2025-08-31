using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse3D: MonoBehaviour
{

    public static Mouse3D Instance { get; private set; }

    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugVisual;
    //[SerializeField] public Cinemachine.CinemachineFreeLook freeLookCamera;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
        {
            transform.position = raycastHit.point;
        }

        if(debugVisual!= null) { debugVisual.position = GetMouseWorldPosition(); }
    }

    public static Vector3 GetMouseWorldPosition() => Instance.GetMouseWorldPosition_Instance();

    private Vector3 GetMouseWorldPosition_Instance()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
        {
            float step = 0.5f;
            Vector3 clampedPosition = new Vector3(
            Snap(raycastHit.point.x, step),
                Snap(raycastHit.point.y, step),
                Snap(raycastHit.point.z, step)
        );
            return clampedPosition;

        }   
        else
        {
            return Vector3.zero;
        }
    }

    private float Snap(float value, float step)
    {
        return Mathf.Round(value / step) * step;
    }
}
