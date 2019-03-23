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
        private Action OnStateEnterListener = (() => { });
        /// <summary>The on state exit listener.</summary>
        private Action OnStateExitListener = (() => { });

        /// <summary>The on state enter listener with stateInfo.</summary>
        private Action<AnimatorStateInfo> OnStateEnterWithStateListener = (v => { });
        /// <summary>The on state exit listener with stateInfo.</summary>
        private Action<AnimatorStateInfo> OnStateExitWithStateListener;

        /// <summary>
        /// Set the state enter event.
        /// </summary>
        /// <param name="action"></param>
        public void SetStateEnterEvent(Action action)
        {
            OnStateEnterListener += action;
        }

        /// <summary>
        /// Set the state enter event with stateInfo.
        /// </summary>
        /// <param name="action"></param>
        public void SetStateEnterEventWithState(Action<AnimatorStateInfo> action)
        {
            OnStateEnterWithStateListener += action;
        }

        /// <summary>
        /// Set the state exit event.
        /// </summary>
        /// <param name="action"></param>
        public void SetStateExitEvent(Action action)
        {
            OnStateExitListener += action;
        }

        /// <summary>
        /// Set the state exit event with stateInfo.
        /// </summary>
        /// <param name="action"></param>
        public void SetStateExitEventWithState(Action<AnimatorStateInfo> action)
        {
            OnStateExitWithStateListener += action;
        }


        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnStateEnterListener();
            OnStateEnterWithStateListener(stateInfo);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnStateExitListener();
            if (OnStateExitWithStateListener != null)
            {
                Debug.Log("called");
                OnStateExitWithStateListener(stateInfo);
            }
        }

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