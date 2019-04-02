using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    public class BackgroundController : MonoBehaviour
    {
        /// <summary>1Waveのバトルステージ背景</summary>
        [Header("Conponents")]
        [SerializeField] SpriteRenderer firstWaveBattleStage;
        /// <summary>バトルステージ背景</summary>
        [SerializeField] SpriteRenderer secondWaveBattleStage;
        /// <summary>バトルステージ背景</summary>
        [SerializeField] SpriteRenderer thirdWaveBattleStage;

        /// <summary>Fading time for iTween animation</summary>
        [Header("Parameters")]
        [SerializeField] float fadingTime = 1f;

        private void Start()
        {
            BattleManager.Instance.m_BehaviourByState.AddListener(state =>
            {
                if (state == BattleManager.StateMachine.State.Init)
                {
                    Initialize();
                }
            });
        }

        /// <summary>
        /// 背景の初期化
        /// </summary>
        public void Initialize()
        {
            var chapter = ChapterManager.Instance.GetChapter();

            // ステージ画像を設定
            firstWaveBattleStage.sprite = chapter.BattleStage[0];
            secondWaveBattleStage.sprite = chapter.BattleStage[1];
            thirdWaveBattleStage.sprite = chapter.BattleStage[2];

            // 全てのステージ画像を透明にする
            firstWaveBattleStage.color = Color.clear;
            secondWaveBattleStage.color = Color.clear;
            thirdWaveBattleStage.color = Color.clear;
        }

        /// <summary>
        /// waveに応じて適切なBattleStageを表示するアニメーションを再生する
        /// </summary>
        /// <param name="callerObject">oncompleteをコールバックさせるオブジェクト</param>
        public void FadingImageOfStage()
        {
            var hashForFadeOut = new Hashtable()
            {
                {"from", Color.white},
                {"to", Color.clear},
                {"time",  fadingTime},
                {"ignoretimescale", true},
            };
            var hashForFadeIn = new Hashtable()
            {
                {"from", Color.clear},
                {"to", Color.white},
                {"time", fadingTime},
                {"ignoretimescale", true},
            };

            // 現在のwaveに応じて適切なBattleStageを表示するアニメーションを再生する
            switch (BattleManager.Instance.m_StateMachine.m_Wave)
            {
                case BattleManager.StateMachine.Wave.FirstWave:
                    iTween.ValueTo(firstWaveBattleStage.gameObject, hashForFadeIn);
                    break;

                case BattleManager.StateMachine.Wave.SecondWave:
                    iTween.ValueTo(firstWaveBattleStage.gameObject, hashForFadeOut);
                    iTween.ValueTo(secondWaveBattleStage.gameObject, hashForFadeIn);
                    break;

                case BattleManager.StateMachine.Wave.LastWave:
                    iTween.ValueTo(secondWaveBattleStage.gameObject, hashForFadeOut);
                    iTween.ValueTo(thirdWaveBattleStage.gameObject, hashForFadeIn);
                    break;
                default:
                    throw new System.ArgumentException();
            }
        }
    }
}