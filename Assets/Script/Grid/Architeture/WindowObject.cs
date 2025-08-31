using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class WindowObject : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    public Window_DoorObjectTypeISO window_DoorObjectTypeISO;
    public HingeJoint hingeJoint;
    private JointMotor motor;
    [SerializeField] private float openSpeed = 200f;
    [SerializeField] private float closeSpeed = 200f;
    private bool isOpen = false;
    private bool firstPick=false;

    public Action<WindowObject> onDestroyed;


    private void OnDestroy()
    {
        onDestroyed?.Invoke(this);
    }

    public string GetWindow_DoorObjectTypeIsoName()
    {
        return window_DoorObjectTypeISO.name;
    }

    private void Awake()
    {
        hingeJoint = GetComponent<HingeJoint>();
        hingeJoint.motor = motor;
        motor.force = 700f;  // Lực quay của motor
        motor.freeSpin = false;
        
    }




    public string GetInteractText()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        
        if (!isOpen && firstPick)
        {
            return "<Close>";
        }
        else  if( isOpen && firstPick) 
        {
            return " <Open>";
        }
        else
        {
            return " <Open>";
        }

    }
    public void Interact()
    {
        ToggleWindow();

        Debug.Log("press");
    }


    public void ToggleWindow()
    {
        isOpen = !isOpen;
        firstPick = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; // Bật động lực học để cửa hoạt động

        JointMotor newMotor = hingeJoint.motor; // Lấy motor hiện tại
        newMotor.force = 700f;
        newMotor.freeSpin = false;
        newMotor.targetVelocity = isOpen ? openSpeed : -closeSpeed;

        hingeJoint.motor = newMotor; // Gán lại motor sau khi sửa
        hingeJoint.useMotor = true;

        StartCoroutine(MonitorDoorMovement(rb));
        //isOpen = !isOpen;
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
