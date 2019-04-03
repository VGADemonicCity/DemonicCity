using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    public class WaveTitle : MonoBehaviour
    {
        [SerializeField] Animator waveAnim;
        [SerializeField] List<AnimationClip> waveAnimClips;

        /// <summary>
        /// ウェーブアニメーション再生,再生に掛かる時間を返す
        /// </summary>
        /// <returns>再生時間</returns>
        public float Play()
        {
            // 現在のウェーブを取得,アタッチされているImageに適切なSpriteを設定しアニメーションを再生
            var currentWave = BattleManager.Instance.m_StateMachine.m_Wave;
            AnimationClip targetClip = waveAnimClips[0];

            switch (currentWave)
            {
                case BattleManager.StateMachine.Wave.FirstWave:
                    targetClip = waveAnimClips[0];
                    waveAnim.CrossFadeInFixedTime("DisplayFirstWave", 0f);
                    break;
                case BattleManager.StateMachine.Wave.SecondWave:
                    targetClip = waveAnimClips[1];
                    waveAnim.CrossFadeInFixedTime("DisplaySecondWave", 0f);
                    break;
                case BattleManager.StateMachine.Wave.LastWave:
                    targetClip = waveAnimClips[2];
                    waveAnim.CrossFadeInFixedTime("DisplayThirdWave", 0f);
                    break;
                default:
                    break;
            }
            return targetClip.length;
        }
    }
}