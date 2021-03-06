using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_and_parry : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    private static readonly int IsSecondAttack = Animator.StringToHash("isSecondAttack");
    private static readonly int ParryAttack = Animator.StringToHash("ParryAttack");
    private static readonly int IsAttack = Animator.StringToHash("isAttack");
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.blocked = true;
        animator.SetBool(IsSecondAttack, true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Player.Instance.blocked == false)
        {
            Player.Instance.OnEnable();
            // Тут нужен будет какой-нибудь спецэффект, типа искр от удара клинками, звук какой-нибудь, хз
            if (animator.GetBool(IsAttack))
            {
                animator.SetBool(IsAttack, false);
                animator.SetBool(ParryAttack, true);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(ParryAttack, false);
        Player.Instance.blocked = false;
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
