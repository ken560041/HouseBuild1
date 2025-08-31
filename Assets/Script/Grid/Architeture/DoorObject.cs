using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorObject : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    //[SerializeField] DoorPosition doorPosition;
    public Window_DoorObjectTypeISO window_DoorObjectTypeISO;
    public HingeJoint hingeJoint;
    private JointMotor motor;
    [SerializeField] private float openSpeed = 200f;
    [SerializeField] private float closeSpeed = 200f;
    private bool isOpen=false;

    public Action<DoorObject> onDestroyed;


    private void OnDestroy()
    {
        onDestroyed?.Invoke(this);
    }

    public string GetWindow_DoorObjectTypeIsoName()
    {
        return window_DoorObjectTypeISO.name;
    }

    public void Awake()
    {
        hingeJoint = GetComponent<HingeJoint>();
        motor = hingeJoint.motor;
        motor.force = 700f;  // Lực quay của motor
        motor.freeSpin = false;
        int doorLayer = LayerMask.NameToLayer("Door");
        int terrainLayer = LayerMask.NameToLayer("Ground");
        Physics.IgnoreLayerCollision(doorLayer, terrainLayer);
    }

    
    public string GetInteractText()
    {
        return "<Open>";
    }
    public void Interact()
    {
        ToggleDoor();

        Debug.Log("press");
    }


    private void ToggleDoor()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            motor.targetVelocity = openSpeed;  // Tốc độ mở cửa
        }
        else
        {
            motor.targetVelocity = -closeSpeed; // Tốc độ đóng cửa
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; // Bật động lực học để cửa hoạt động

        StartCoroutine(MonitorDoorMovement(rb));
        hingeJoint.motor = motor;
        hingeJoint.useMotor = true;  // Kích hoạt motor
    }


    private IEnumerator MonitorDoorMovement(Rigidbody rb)
    {
        float targetAngle = isOpen ? hingeJoint.limits.max : hingeJoint.limits.min;

        while (true)
        {
            float currentAngle = hingeJoint.angle;

            // Khi cửa gần đạt giới hạn (±1 độ), dừng motor và kinematic
            if (Mathf.Abs(currentAngle - targetAngle) < 1f)
            {
                hingeJoint.useMotor = false;
                rb.isKinematic = true;
                break;
            }

            yield return null;
        }
    }

}
