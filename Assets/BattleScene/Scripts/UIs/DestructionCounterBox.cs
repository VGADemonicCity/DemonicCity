using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    public class DestructionCounterBox : MonoBehaviour
    {
        [SerializeField] float fadingTime = 1f;

        TypefaceAnimator typefaceAnimator;
        Text valueTextBox;
        int count;


        private void Awake()
        {
            typefaceAnimator = GetComponentInChildren<TypefaceAnimator>();
            valueTextBox = GetComponentInChildren<Text>();

            gameObject.transform.localScale = Vector3.zero;
        }

        private void Start()
        {
            var battleManager = BattleManager.Instance;
            battleManager.m_BehaviourByState.AddListener(state =>
            {
                if (state == BattleManager.StateMachine.State.PlayerChoice && !battleManager.m_StateMachine.PreviousStateIsPause)
                {
                    Initialize();
                }
            });
        }

        void Initialize()
        {
            count = 0;
            IsVisible();
        }

        bool IsVisible()
        {
            if (count > 0)
            {
                if (gameObject.transform.localScale == Vector3.zero)
                {
                    iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.one, "time", fadingTime));
                }
                return true;
            }
            else
            {
                if (gameObject.transform.localScale == Vector3.one)
                {
                    iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.zero, "time", fadingTime));
                }
                return false;
            }
        }

        public void OnPanelCount(int addCount)
        {
            count += addCount;
            valueTextBox.text = string.Format("×{0}", count);
            IsVisible();
            typefaceAnimator.Play();
        }
    }
}