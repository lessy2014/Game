using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_mage_script : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //
    // }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // var rotation = Quaternion.Euler(0, 0, 0);
        // Вот эта шляпа высирает шляпы из под шляпника, когда он телепортируется. Если сильно захочется, можно будет в настройках запилить опцию для включения этого *clown*
        // if (!Koldun.Instance.isRight)
        // {
        //     rotation.y = 180;
        // }
        // Instantiate(Koldun.Instance.hat, Koldun.Instance.transform.position, rotation);
        // if (Koldun.Instance.isRight)
        //     Koldun.Instance.transform.position = new Vector3(Player.Instance.transform.position.x-1f,
        //         Player.Instance.transform.position.y, Player.Instance.transform.position.z);
        // else
        //     Koldun.Instance.transform.position = new Vector3(Player.Instance.transform.position.x+1f,
        //         Player.Instance.transform.position.y, Player.Instance.transform.position.z);
        Koldun.Instance.transform.position = Player.Instance.groundCheck.position;
        animator.Play("get_back_mage");
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
