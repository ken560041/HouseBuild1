using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCamera : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform playerTransform;
 
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsCameraInside())
        {
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Walls"));
        }
        else
        {
            Camera.main.cullingMask |= (1 << LayerMask.NameToLayer("Walls"));
        }
    }


    bool IsCameraInside()
    {
        // Ví dụ đơn giản kiểm tra vị trí camera
        return Camera.main.transform.position.y < 2.0f;
    }
}
