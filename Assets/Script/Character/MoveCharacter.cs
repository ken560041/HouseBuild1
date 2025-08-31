using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class MoveCharacter : MonoBehaviour
{
    // Start is called before the first frame update


    public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    private InputManage _inputManage;

    private Transform cameraMainTransform;


    [Header("CinemachineCamera")]
    public GameObject CM1;
    public GameObject CM2;

    private void Start()
    {
        /*controller = gameObject.GetComponent<CharacterController>();*/
        _inputManage=InputManage.Instance;
        cameraMainTransform = Camera.main.transform;
    }

    void Update()
    {
        
        groundedPlayer = controller.isGrounded;

        //Debug.Log(this.gameObject.name);
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = _inputManage.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move =cameraMainTransform.forward*move.z+cameraMainTransform.right*move.x;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (_inputManage.PlayerJumpedThisFrame() && groundedPlayer)
        {
             playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);


        }

        if (_inputManage.SwitchCameraThis())
        {
            if (CM1.active == true)
            {
                CM1.active= false;
                CM2.active = true;
            }
            else if(CM2.active == true)
            {
                CM2.active = false;
                CM1.active = true;
            }
            
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
