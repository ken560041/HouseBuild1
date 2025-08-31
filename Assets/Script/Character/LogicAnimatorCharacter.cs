using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicAnimatorCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    Animator character;
    int isJumpHash;
    private InputManage _inputManage;
    void Start()
    {
        character= GetComponent<Animator>();
        _inputManage = InputManage.Instance;
        isJumpHash = Animator.StringToHash("isJump");
    }

    // Update is called once per frame
    void Update()
    {
        MoveAnimator();
        AttackAnimator();
        
    }

    void MoveAnimator() {

        bool jumpPressed = _inputManage.PlayerJumpedThisFrame();
        bool isJumping=character.GetBool(isJumpHash);

        Vector2 movement = _inputManage.GetPlayerMovement();
        
        
        if(jumpPressed && !isJumping)
        {
            character.SetBool(isJumpHash, true);
        }
        else if(!jumpPressed && isJumping) {
            character.SetBool(isJumpHash, false);
        }

        if(movement.sqrMagnitude > 0) {
            character.SetFloat("Speed", 1); 

        }
        else if(movement.sqrMagnitude == 0) {
            character.SetFloat("Speed", 0);
        }



        




    }


    void AttackAnimator()
    {
        bool attackPressed = _inputManage.IsLeftClickTriggered();

        if(attackPressed)
        {
            character.SetTrigger("attack");
        }


    }

}
