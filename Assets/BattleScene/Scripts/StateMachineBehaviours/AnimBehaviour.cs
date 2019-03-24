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
        /// <summary>The on state eexitxit listener.</summary>
        private Action OnStateExitListener = (() => { });
        /// <summary>The on state update listener.</summary>
        private Action OnStateUpdateListener = (() => { });
        /// <summary>The on state move listener.</summary>
        private Action OnStateMoveListener = (() => { });
        /// <summary>The on state stateIK listener.</summary>
        private Action OnStateStateIKListener = (() => { });

        /// <summary>The on state enter listener with stateInfo.</summary>
        private Action<AnimatorStateInfo> OnStateEnterWithStateListener = (v => { });
        /// <summary>The on state exit listener with stateInfo.</summary>
        private Action<AnimatorStateInfo> OnStateExitWithStateListener = (v => { });
        /// <summary>The on state exit listener.</summary>
        private Action<AnimatorStateInfo> OnStateUpdateWithStateListener = (v => { });
        /// <summary>The on state exit listener.</summary>
        private Action<AnimatorStateInfo> OnStateMoveWithStateListener = (v => { });
        /// <summary>The on state exit listener.</summary>
        private Action<AnimatorStateInfo> OnStateStateIKWithStateListener = (v => { });

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

        /// <summary>
        /// Set the state update event.
        /// </summary>
        /// <param name="action"></param>
        public void SetStateUpdateEvent(Action action)
        {
            OnStateUpdateListener += action;
        }

        /// <summary>
        /// Set the state update event with stateInfo.
        /// </summary>
        /// <param name="action"></param>
        public void SetStateUpdateEventWithState(Action<AnimatorStateInfo> action)
        {
            OnStateUpdateWithStateListener += action;
        }

        /// <summary>
        /// Set the state move event.
        /// </summary>
        /// <param name="action"></param>
        public void SetStateMoveEvent(Action action)
        {
            OnStateMoveListener += action;
        }

        /// <summary>
        /// Set the state move event with stateInfo.
        /// </summary>
        /// <param name="action"></param>
        public void SetStateMoveEventWithState(Action<AnimatorStateInfo> action)
        {
            OnStateMoveWithStateListener += action;
        }

        /// <summary>
        /// Set the state StateIK event.
        /// </summary>
        /// <param name="action"></param>
        public void SetStateIKExitEvent(Action action)
        {
            OnStateStateIKListener += action;
        }

        /// <summary>
        /// Set the state StateIK event with stateInfo.
        /// </summary>
        /// <param name="action"></param>
        public void SetStateIKEventWithState(Action<AnimatorStateInfo> action)
        {
            OnStateStateIKWithStateListener += action;
        }


        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnStateEnterListener();
            OnStateEnterWithStateListener(stateInfo);
        }

        //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnStateUpdateListener();
            OnStateUpdateWithStateListener(stateInfo);
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnStateExitListener();
            OnStateExitWithStateListener(stateInfo);
        }

        // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
        override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnStateMoveListener();
            OnStateMoveWithStateListener(stateInfo);
        }

        // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnStateStateIKListener();
            OnStateStateIKWithStateListener(stateInfo);
        }
    }
}