using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCaster : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Settings")]
    public LayerMask obstacleMask; // Layer chứa tường, vật cản
    public float maxDistance = 1000f;

    public Vector3? currentHitPoint = null;
    public float cameraDistance = 0f;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, obstacleMask))
        {
            currentHitPoint = hit.point;
            cameraDistance = Vector3.Distance(Camera.main.transform.position, hit.point);
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        else
        {
            currentHitPoint = null;
            cameraDistance = maxDistance;
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * maxDistance, Color.green);
        }
    }

    public bool HasHit => currentHitPoint.HasValue;
    public Vector3 GetHitPointOrDefault(Vector3 defaultPoint) => currentHitPoint ?? defaultPoint;
}
