using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : State
{
    float gravityValues;
    Vector3 currentVelocity;
    bool grounded;
    float playerSpeed;
    bool attack;

    bool sheathWeapon;

    Vector3 cVelocity;

    public CombatState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }




    public override void Enter()
    {
        base.Enter();


        sheathWeapon = false;
        input = Vector2.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0f;
        attack = false;


        velocity = character.playerVelocity;
        playerSpeed=character.playerSpeed;
        grounded = character.controller.isGrounded;
        gravityValues = character.gravityValue;

    }


    public override void HandleInput()
    {
        base.HandleInput();

        if (drawWeaponAction.triggered)
        {
            sheathWeapon = true;

        }


        if (attackAction.triggered)
        {
            attack = true;
        }

        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);

        velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;

        velocity.y = 0;



    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        character.animator.SetFloat("speed", input.magnitude, character.speedDampTime, Time.deltaTime);

        if(sheathWeapon)
        {
            character.animator.SetTrigger("sheathSword");
            stateMachine.ChangeState(character.standing);
        }

        if (attack)
        {
            character.animator.SetTrigger("attack");
            stateMachine.ChangeState(character.attacking);
        }
      




    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        gravityVelocity.y += gravityValues * Time.deltaTime;
        grounded = character.controller.isGrounded;

        if (grounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = 0f;
        }

        
        currentVelocity = Vector3.Lerp(currentVelocity, velocity, character.velocityDampTime);
        character.controller.Move(currentVelocity * Time.deltaTime * playerSpeed + gravityVelocity * Time.deltaTime);

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity), character.rotationDampTime);
        }

    }

    public override void Exit()
    {
        base.Exit();

        gravityVelocity.y = 0f;
        sheathWeapon = false;
        character.playerVelocity = new Vector3(input.x, 0, input.y);

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.LookRotation(velocity);
        }

    }

}
