using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingState : State
{
    // Start is called before the first frame update

    float timePassed;
    float landingTime;
    public LandingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        character.playerVelocity = Vector3.zero;
        timePassed = 0f;
        character.animator.SetTrigger("land");
        landingTime = 0.1f;
    }

    public override void LogicUpdate()
    {

        base.LogicUpdate();
        if (timePassed > landingTime)
        {
            character.animator.SetTrigger("move");
            stateMachine.ChangeState(character.standing);
        }
        timePassed += Time.deltaTime;
    }


}
