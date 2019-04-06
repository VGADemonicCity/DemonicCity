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

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_BehaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.NextWave) // StateがWin以外の時は処理終了
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
            m_enemyHPGauge.Initialize(m_battleManager.CurrentEnemy.Stats.Temp.HitPoint); // 敵のHP最大値をGaugeに登録する
            StartCoroutine(m_enemyHPGauge.FullGameDrawing()); // ゲージを最大に戻す
            m_enemiesMover.Moving(); // 待機させている次の敵を最前面迄動かす
            backgroundCtrl.FadingImageOfTheStage(); // ステージ背景を遷移させるアニメーション再生

            var waitTime = waveTitle.Play();
            // 指定秒数分遅延させた後ステートマシンを状態遷移させる
            yield return new WaitForSeconds(waitTime);
            m_battleManager.SetStateMachine(BattleManager.StateMachine.State.PlayerChoice);
        }
    }
}

