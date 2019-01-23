using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace DemonicCity.BattleScene
{
    public abstract class PopupSystem : MonoBehaviour
    {
        // First process, Play "Start" animation.
        // Any processes, Popup system execution.
        // Last process, Play "Exit" animation.

        public delegate void Execution();
        public delegate bool Confirmation();
        public event Execution Execute;
        public event Confirmation Confirm;

        BattleManager battleManager;
        [SerializeField] public Animator animator;
        bool isActivating;
        BattleManager.StateMachine.State stateBuffer;
        const string parameter = "Switch";

        protected virtual void OnEnable()
        {
            battleManager = BattleManager.Instance;

            if (!animator)
            {
                animator = GetComponent<Animator>();
            }
        }

        protected virtual void Update()
        {
            if (!isActivating)
            {
                return;
            }

            if (Execute != null)
            {
                Execute();
                Execute -= Execute;
            }

            if(Confirm != null)
            {
                if(Confirm())
                {
                    Allowed();
                }
                else
                {
                    Denied();
                }
                Confirm -= Confirm;
            }
        }

        /// <summary>
        /// ポップアップウィンドウを表示
        /// </summary>
        public virtual void Popup()
        {
            stateBuffer = battleManager.m_StateMachine.m_State;
            battleManager.SetStateMachine(BattleManager.StateMachine.State.Pause);
            animator.SetTrigger(parameter);
            isActivating = true;
        }

        /// <summary>
        /// ポップアップウィンドウを閉じる
        /// </summary>
        public virtual void Close()
        {
            isActivating = false;
            animator.SetTrigger(parameter);
            battleManager.SetStateMachine(stateBuffer);
        }

        /// <summary>Psitiveな入力があった時</summary>
        protected abstract void Allowed();
        /// <summary>negativeな入力があった時</summary>
        protected abstract void Denied();
    }
}