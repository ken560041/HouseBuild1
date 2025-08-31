using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraObstacleAdjuster : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float minDistance = 2f;
    public float maxDistance = 10f;

    private CinemachineVirtualCamera vCam;
    private Cinemachine3rdPersonFollow thirdPersonFollow;

    void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        thirdPersonFollow = vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        if (thirdPersonFollow == null)
        {
            Debug.LogError("CameraZoomByScroll: 3rdPersonFollow not found! Set Body = '3rd Person Follow' in the Virtual Camera.");
            enabled = false;
        }
    }

    void Update()
    {
        if (thirdPersonFollow == null) return;

        float scrollInput = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            float newDistance = thirdPersonFollow.CameraDistance - scrollInput * zoomSpeed;
            thirdPersonFollow.CameraDistance = Mathf.Clamp(newDistance, minDistance, maxDistance);
        }
    }
}