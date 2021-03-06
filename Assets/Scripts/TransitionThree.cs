using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionThree : StateMachineBehaviour
{
    private static readonly int IsAttack = Animator.StringToHash("isAttack");
    private static readonly int IsSecondAttack = Animator.StringToHash("isSecondAttack");
    // private static readonly int IsRunning = Animator.StringToHash("isRunning");
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool(IsAttack))
            animator.Play("NEW shortFirstAttack");
        // if (animator.GetBool(IsRunning))
        //     animator.Play("NEW runningWithSword 0");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(IsAttack, false);
        animator.SetBool(IsSecondAttack, false);
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
