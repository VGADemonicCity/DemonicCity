using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Next wave state.
    /// </summary>
    public class NextWaveState : StatesBehaviour
    {
        /// <summary>敵のオブジェクト群を動かすクラス</summary>
        [SerializeField] EnemiesMover m_enemiesMover;
        /// <summary>ステージ背景を制御するクラス</summary>
        [SerializeField] BackgroundController backgroundCtrl;
        /// <summary>animator of wave title</summary>
        [SerializeField] WaveTitle waveTitle;
        [SerializeField] MagiaAudioPlayer magiaAudioPlayer;


        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_BehaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.NextWave || m_battleManager.m_StateMachine.PreviousStateIsPause) // StateがNextWave以外の時は処理終了
                {
                    return;
                }
                StartCoroutine(WaveTransition());
            });
        }

        IEnumerator WaveTransition()
        {
            m_battleManager.m_StateMachine.m_Wave++;
            m_battleManager.CurrentEnemy.Stats.Init(m_battleManager.CurrentEnemy.Stats); //バトル開始直前の 敵のステータスの初期値を保存
            m_enemyHPGauge.Initialize(m_battleManager.CurrentEnemy.Stats.HitPoint); // 敵のHP最大値をGaugeに登録する
            StartCoroutine(m_enemyHPGauge.FullGameDrawing()); // ゲージを最大に戻す
            backgroundCtrl.FadingImageOfTheStage(); // ステージ背景を遷移させるアニメーション再生

            // 待機させている次の敵を最前面迄動かす
            var waitTime = m_enemiesMover.Moving();
            yield return new WaitForSeconds(waitTime);

            // LastWaveの時章に応じたマギアの音声を再生する
            switch (m_battleManager.m_StateMachine.m_Wave)
            {
                case BattleManager.StateMachine.Wave.LastWave:
                    waitTime = magiaAudioPlayer.PlayBattleBossVoice(m_chapter);
                    yield return new WaitForSeconds(waitTime);
                    break;
                default:
                    break;
            }

            waitTime = waveTitle.Play();
            // 指定秒数分遅延させる
            yield return new WaitForSeconds(waitTime);

            switch (m_battleManager.m_StateMachine.m_Wave)
            {
                case BattleManager.StateMachine.Wave.LastWave:
                    SoundManager.Instance.PlayWithFade(SoundManager.SoundTag.BGM, m_chapter.BossBgm);
                    break;
                default:
                    break;
            }

            // ステートマシンを状態遷移させる
            m_battleManager.SetStateMachine(BattleManager.StateMachine.State.PlayerChoice);
        }
    }
}

