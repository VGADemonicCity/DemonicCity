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

        void OnCompleted()
        {
            
        }


        public IEnumerator PanelCompleteSkillAnimation()
        {
            yield return new WaitForSeconds(CuttingIn());
            yield return new WaitForSeconds(PlaySkillAnimation());

        }
    }
}