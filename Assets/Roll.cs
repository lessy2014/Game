using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : StateMachineBehaviour
{
    private static readonly int IsSecondAttack = Animator.StringToHash("isSecondAttack");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(IsSecondAttack, Player.Instance.swordInJump);
        // animator.SetBool(IsRunning, true);
        if (Player.Instance.right)
            Player.Instance.movementX = 1.5f;
        else
            Player.Instance.movementX = -1.5f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Player.Instance.movementX = 0;
        Player.Instance.rolling = false;
        if (!animator.GetBool(IsRunning))
            Player.Instance.movementX = 0;
        else if (Player.Instance.right)
            Player.Instance.movementX = 1f;
        else
            Player.Instance.movementX = -1f;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
