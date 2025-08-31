using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class  ThirdPersonController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject CinemachineCameraTarget;

    /*[SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private float verticalSpeed = 3.0f;*/
    public bool LockCameraPosition=false;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;

    // Update is called once per frame
    void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {   
        Vector2 mouseDelta = InputManage.Instance.GetMouse();

        if(mouseDelta.sqrMagnitude>0.01f&& !LockCameraPosition)
        {
            _cinemachineTargetYaw+=mouseDelta.x*Time.deltaTime;
            _cinemachineTargetPitch += mouseDelta.y * Time.deltaTime;
        }

        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);


        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

}
