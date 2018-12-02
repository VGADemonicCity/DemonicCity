using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Player attack state.
    /// 攻撃処理を行うクラス
    /// </summary>
    public class PlayerAttackState : StatesBehaviour
    {
   

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_behaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.PlayerAttack) // StateがPlayerAttack以外の時は処理終了
                {
                    return;
                }
                m_battleManager.m_stateMachine.Save();
                // ==============================
                // イベント呼び出し : SkillJudger
                // ==============================
                m_skillManager.m_skillJudger.Invoke(m_magia.MyPassiveSkill , m_panelCounter.GetCityDestructionCount()); // SkillManagerのイベントを呼び出してPassiveSkillをステータスに反映させる
                StartCoroutine(AttackProcess()); // 攻撃プロセスを開始する
            });
        }

        /// <summary>
        /// スキル効果反映後、攻撃処理と演出を行う
        /// </summary>
        /// <returns>The process.</returns>
        IEnumerator AttackProcess()
        {
            // ==============================
            // ここに攻撃の演出処理を入れる予定
            // ==============================
            Debug.Log("attack process called.");
            yield return new WaitForSeconds(3f);

            yield return new WaitWhile(() => // falseになるまで待つ
            {
                Debug.Log("PlayerAttack state called.");
                // ==================================
                // イベント呼び出し : StateMachine.
                // ==================================
                bool enemyIsAlive = true; // 敵がまだ生きていたら敵の攻撃ターン。死んでいればWinステートへ遷移
                if (enemyIsAlive)
                {
                    // ==================================
                    // イベント呼び出し : StateMachine.EnemyAttack
                    // ==================================
                    SetStateMachine(BattleManager.StateMachine.State.EnemyAttack);
                }
                else
                {
                    // ==================================
                    // イベント呼び出し : StateMachine.Win
                    // もし次のWaveが存在すれば、次のWaveへ遷移する処理を書く
                    // ==================================
                    SetStateMachine(BattleManager.StateMachine.State.Win);
                }
                return false;
            });
        }
    }
}
