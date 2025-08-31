using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class StandingState : State
{
   
    float gravityValue;
    bool jump;
    bool crouch;
    Vector3 currentVelocity;
    bool grounded;
    bool sprint;
    float playerSpeed;
    bool drawWeapon;
    private float _rotationVelocity;

    Vector3 cVelocity;
    Vector3 targetDirection;
    public StandingState(Character _character, StateMachine _stateMachine): base(_character, _stateMachine) 
    {
        character = _character;
        stateMachine = _stateMachine;
    }


    public override void Enter()
    {
        base.Enter();
        jump= false;
        crouch = false;
        drawWeapon = false;
        input=Vector2.zero;

        currentVelocity = Vector3.zero;

        gravityVelocity.y = 0;
        velocity = character.playerVelocity;
        playerSpeed = character.playerSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;
    }


    public override void HandleInput()
    {
        base.HandleInput();

        if(jumpAction.triggered)
        {

            jump= true;
        }
        if(crouchAction.triggered)
        {

            crouch= true;
        }

        if(drawWeaponAction.triggered)
        {
            drawWeapon= true;
        }

        input = moveAction.ReadValue<Vector2>();
        
        
        /*Vector3 camForward = character.cameraTransform.forward;
        Vector3 camRight = character.cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        velocity = (input.x * camRight + input.y * camForward).normalized;   
        velocity.y = 0f;*/
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();


        character.animator.SetFloat("speed", input.magnitude, character.speedDampTime, Time.deltaTime);

        if (jump)
        {
            stateMachine.ChangeState(character.jumping);
        }

        if(crouch)
        {

            stateMachine.ChangeState(character.crouching);
        }


        if(drawWeapon)
        {
            stateMachine.ChangeState(character.combatting);
            character.animator.SetTrigger("drawSword");

        }
    }
    public override void PhysicsUpdate()
    {
  
        base.PhysicsUpdate();
        Vector3 moveDir;

        Vector3 camForward = (character.transform.position-Camera.main.transform.position);

        camForward.y = 0;
        camForward.Normalize();
        Vector3 camRight = Vector3.Cross(Vector3.up, camForward); ;



        if (input != Vector2.zero)
        {

            moveDir = input.y*camForward + input.x*camRight;

            /*Vector3 moveDirYe = moveDir.x * character.transform.forward+ moveDir.z*character.transform.right;*/
            character.controller.Move(moveDir.normalized * playerSpeed * Time.deltaTime);
            character.transform.forward = moveDir;
            
        }



        


       
        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;

        if (grounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = 0f;
        }

        currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity, ref cVelocity, character.velocityDampTime);
        character.controller.Move(currentVelocity * Time.deltaTime * playerSpeed + gravityVelocity * Time.deltaTime);

        

        if (velocity.sqrMagnitude > 0)
        {
            

        }

    }

    public override void Exit()
    {
        base.Exit();

        gravityVelocity.y = 0f;
        character.playerVelocity = new Vector3(input.x, 0, input.y);

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.LookRotation(velocity);
        }
    }
}
