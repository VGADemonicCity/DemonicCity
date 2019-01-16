using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Animation behaviour.
    /// </summary>
    public class AnimBehaviour : StateMachineBehaviour
    {
        /// <summary>The on state enter listener.</summary>
        private Action OnStateEnterListener;

        private Action<AnimatorStateInfo> OnStateEnterWithStateListener;

        public void SetStateEnterEvent(Action action)
        {
            OnStateEnterListener += action;
        }

        public void SetStateEnterEventWithState(Action<AnimatorStateInfo> action)
        {
            OnStateEnterWithStateListener += action;
        }

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(OnStateEnterListener != null)
            {
                OnStateEnterListener();
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}
    }


}