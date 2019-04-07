using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        /// <summary>cityNameBoxのテキストコンポーネント</summary>
        Text cityNameTextBox;

        /// <summary>Fading time for iTween animation</summary>
        [Header("Parameters")]
        [SerializeField] float fadingTime = 1f;

        Chapter chapter;


        private void Awake()
        {
            BattleManager.Instance.m_BehaviourByState.AddListener(state =>
            {
                if (state == BattleManager.StateMachine.State.Init)
                {

                    // 初期化を行い1wave目のStageを表示するアニメーションを再生
                    Initialize();
                    FadingImageOfTheStage();
                }
            });
        }

        /// <summary>
        /// 背景の初期化
        /// </summary>
        public void Initialize()
        {
            var chapter = ChapterManager.Instance.GetChapter();
            //chapter = ChapterManager.Instance.GetChapter(Progress.StoryProgress.Nafla);

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
        /// <returns>アニメーションに掛ける時間を返す</returns>
        public float FadingImageOfTheStage()
        {
            // 現在のwaveに応じて適切なBattleStageを表示するアニメーションを再生する
            switch (BattleManager.Instance.m_StateMachine.m_Wave)
            {
                case BattleManager.StateMachine.Wave.FirstWave:
                    iTween.ValueTo(firstWaveBattleStage.gameObject, GenerateHash(firstWaveBattleStage.gameObject, Fade.In));
                    break;
                case BattleManager.StateMachine.Wave.SecondWave:
                    iTween.ValueTo(firstWaveBattleStage.gameObject, GenerateHash(firstWaveBattleStage.gameObject, Fade.Out));
                    iTween.ValueTo(secondWaveBattleStage.gameObject, GenerateHash(secondWaveBattleStage.gameObject, Fade.In));
                    break;
                case BattleManager.StateMachine.Wave.LastWave:
                    iTween.ValueTo(secondWaveBattleStage.gameObject, GenerateHash(secondWaveBattleStage.gameObject, Fade.Out));
                    iTween.ValueTo(thirdWaveBattleStage.gameObject, GenerateHash(thirdWaveBattleStage.gameObject, Fade.In));
                    break;
                default:
                    throw new System.ArgumentException();
            }
            return fadingTime;
        }

        /// <summary>
        /// iTweenアニメーション用のハッシュテーブルを生成する
        /// </summary>
        /// <param name="targetGO"></param>
        /// <param name="fade"></param>
        /// <returns></returns>
        Hashtable GenerateHash(GameObject targetGO, Fade fade)
        {
            var clear = new Color(1, 1, 1, 0);

            switch (fade)
            {
                case Fade.In:
                    return new Hashtable()
                    {
                        {"from", clear},
                        {"to", Color.white},
                        {"time", fadingTime},
                        {"onupdate","OnUpdateCallBack" },
                        {"onupdatetarget",targetGO},
                    };
                case Fade.Out:
                    return new Hashtable()
                    {
                        { "from", Color.white},
                        {"to", clear},
                        {"time", fadingTime},
                        {"onupdate","OnUpdateCallBack" },
                     {"onupdatetarget",targetGO},
                     };
                default:
                    return null;
            }
        }

        /// <summary>
        /// フェードさせる先
        /// </summary>
        enum Fade
        {
            In,
            Out,
        }
    }
}