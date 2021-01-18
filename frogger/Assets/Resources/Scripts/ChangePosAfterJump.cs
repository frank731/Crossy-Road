using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePosAfterJump : StateMachineBehaviour
{
    private PlayerController playerController = null;
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(playerController == null)
        {
            playerController = animator.gameObject.GetComponent<PlayerController>();
        }
        
        //move player parent object so player doesnt teleport back to jump animation start point
        playerController.Move();

    }
}
