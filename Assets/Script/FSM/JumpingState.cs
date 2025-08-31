using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : State
{
    // Start is called before the first frame update


    bool grounded;
    float gravityValue;
    float jumpHeight;
    float playerSpeed;

    Vector3 airVeclocity;


   public JumpingState(Character _character, StateMachine _stateMachine): base(_character, _stateMachine) 
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        grounded = false;
        gravityValue=character.gravityValue;
        jumpHeight = character.jumpHeight;
        playerSpeed = character.playerSpeed;

        gravityVelocity.y = 0;


        character.animator.SetFloat("speed", 0);
        character.animator.SetTrigger("jump");

        Jump();

    }

    public override void HandleInput()
    {
        base.HandleInput();

        input = moveAction.ReadValue<Vector2>();

    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();
    
        if(grounded)
        {
            stateMachine.ChangeState(character.landing);
        }
    
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!grounded)
        {

            Vector3 camForward = (character.transform.position - Camera.main.transform.position);

            camForward.y = 0;
            camForward.Normalize();
            Vector3 camRight = Vector3.Cross(Vector3.up, camForward); ;

            velocity = character.playerVelocity;
            airVeclocity = new Vector3(input.x, 0, input.y);
            velocity = velocity.x * camRight + velocity.z * camForward;

            velocity.y = 0f;

            airVeclocity= airVeclocity.x * character.cameraTransform.right.normalized + airVeclocity.z * character.cameraTransform.forward.normalized;

            airVeclocity.y = 0.0f;
            character.controller.Move(gravityVelocity*Time.deltaTime+(airVeclocity*character.airControl+velocity*(1-character.airControl))*playerSpeed*Time.deltaTime);
        }

        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;

        

        // Nếu đã tiếp đất → reset vận tốc rơi
        if (grounded)
        {
            gravityVelocity = Vector3.zero;
            airVeclocity = Vector3.zero;
        }

    }
    void Jump()
    {
        gravityVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }
}
