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
        bool isSkipEffect;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            isSkipEffect = Debugger.BattleDebugger.Instance.EffectSkip;
        }

        float CuttingIn()
        {
            animator.CrossFadeInFixedTime(cutIn.name, 0f);
            return cutIn.length;
        }

        float PlaySkillAnimation()
        {
            animator.CrossFadeInFixedTime(skill.name, 0);
            return skill.length;
        }

        public void PlayCompleteSE()
        {
            GetComponent<AudioSource>().Play();
        }

        /// <summary>
        /// パネルコンプリート時のアニメーション再生し,プレイヤーの攻撃ステートへ遷移
        /// </summary>
        /// <returns></returns>
        public IEnumerator PlayPanelCompleteSkillAnimation()
        {
            if (!Debugger.BattleDebugger.Instance.EffectSkip)
            {
                yield return new WaitForSeconds(CuttingIn());
                yield return new WaitForSeconds(PlaySkillAnimation());
            }
            // =========================================
            // イベント呼び出し : StateMachine.PlayerAttack
            // =========================================
            BattleManager.Instance.SetStateMachine(BattleManager.StateMachine.State.PlayerAttack);
        }
    }
}