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
        [SerializeField] AudioClip cutinVoice;
        [SerializeField] AudioClip skillActivatingSE;
        [SerializeField] AudioClip skillActivatingVoice;

        SoundManager soundManager;


        Animator animator;
        bool isSkipEffect;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            isSkipEffect = Debugger.BattleDebugger.Instance.EffectSkip;
            soundManager = SoundManager.Instance;
        }

        /// <summary>
        /// カットインアニメーション再生
        /// </summary>
        /// <returns></returns>
        float CuttingIn()
        {
            animator.CrossFadeInFixedTime(cutIn.name, 0f);
            return cutIn.length;
        }

        /// <summary>
        /// 朱雀炎掌スキルアニメーション再生
        /// </summary>
        /// <returns></returns>
        float PlaySkillAnimation()
        {
            animator.CrossFadeInFixedTime(skill.name, 0);
            return skill.length;
        }

        /// <summary>
        /// 朱雀炎掌のSE再生
        /// </summary>
        /// <returns></returns>
        public void PlayPerfectSkillVoice()
        {
            soundManager.PlayWithFade(SoundManager.SoundTag.Voice, skillActivatingVoice);
        }

        /// <summary>
        /// 朱雀炎掌発動時のvoice再生
        /// </summary>
        /// <returns></returns>
        public void PlayPerfectSkillSE()
        {
            soundManager.PlayWithFade(SoundManager.SoundTag.SE, skillActivatingSE);
        }

        /// <summary>
        /// カットイン時のvoice再生
        /// </summary>
        /// <returns></returns>
        public void PlayCutInVoice()
        {
            soundManager.PlayWithFade(SoundManager.SoundTag.Voice, cutinVoice);
        }

        public void StopSE()
        {
            soundManager.StopWithFade(SoundManager.SoundTag.SE);
        }

        public void StopVoice()
        {
            soundManager.StopWithFade(SoundManager.SoundTag.Voice);
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