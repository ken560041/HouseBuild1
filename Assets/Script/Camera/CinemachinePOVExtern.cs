using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CinemachinePOVExtern : CinemachineExtension
{
    // Start is called before the first frame update


    [SerializeField]
    private float horizontalSpeed = 100f;

    [SerializeField]
    private float verticalSpeed = 100f;

    [SerializeField]
    private float clampAngle = 80f;

    private InputManage _inputManage;
    private Vector3 startingRotation;

    protected override void Awake()
    {

        _inputManage = InputManage.Instance;
        base.Awake();
    }
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        


        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null)
                {
                    startingRotation = transform.localRotation.eulerAngles;

                }

                Vector2 deltaInput = _inputManage.GetMouse();
                startingRotation.x += deltaInput.x *Time.deltaTime;
                startingRotation.y += deltaInput.y * Time.deltaTime;

                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x,0);
                //state.RawOrientation = Quaternion.Euler(0,0,0);

            }
        }


    }

}
