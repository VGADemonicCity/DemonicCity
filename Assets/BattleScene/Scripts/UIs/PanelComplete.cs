using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// パネルコンプリート時の演出クラス
    /// </summary>
    public class PanelComplete : MonoBehaviour
    {
        [SerializeField] AnimationClip cutIn;
        [SerializeField] AnimationClip skill;
        Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            BattleManager.Instance.m_BehaviourByState.AddListener(state =>
            {
                if(state != BattleManager.StateMachine.State.PlayerChoice)
                {
                    return;
                }


            });
        }

        public float CuttingIn()
        {
            animator.CrossFadeInFixedTime(cutIn.name, 0f);
            return cutIn.length;
        }

        public float PlaySkillAnimation()
        {
            animator.CrossFadeInFixedTime(skill.name, 0);
            return skill.length;
        }


        public IEnumerator PanelCompleteSkillAnimation()
        {
            yield return new WaitForSeconds(CuttingIn());
            PlaySkillAnimation();
        }
    }
}