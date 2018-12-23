using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Enemy attack state.
    /// </summary>
    public class EnemyAttackState : StatesBehaviour
    {
        /// <summary>MagiaのHPDrawの参照</summary>
        [SerializeField] HitPointGauge m_magiaHPGauge;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_behaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if (state != BattleManager.StateMachine.State.EnemyAttack) // StateがEnemyAttack以外の時は処理終了
                {
                    return;
                }

                Debug.Log("EnemyAttack state called.");
                StartCoroutine(AttackProcess());
            });
        }

        IEnumerator AttackProcess()
        {
            // ==============================
            // ここに攻撃の演出処理を入れる予定
            // ==============================
            Debug.Log("attack process called.");
            yield return new WaitForSeconds(1f);


            Debug.Log("敵から攻撃される前の[" + m_magia + "]の体力 : " + m_battleManager.m_magia.m_hitPoint);
            //yield return new WaitForSeconds(1f);



            yield return new WaitWhile(() => // falseになるまで待つ
            {
                Debug.Log("PlayerAttack state called.");
                var damage = m_battleManager.m_enemy.Stats.m_attack - m_battleManager.m_magia.m_defense; // 敵の攻撃力からプレイヤーの防御力を引いた値
                if(damage > 0)
                {
                    m_battleManager.m_magia.m_hitPoint -= damage; // ダメージ
                    m_magiaHPGauge.Sync(m_battleManager.m_magia.m_hitPoint); // HPGaugeと同期
                }
                Debug.Log("敵から攻撃された後の[" + m_magia + "]の体力 : " + m_battleManager.m_magia.m_hitPoint);

                return false;
            });


            if (m_battleManager.m_magia.m_hitPoint > 0) // プレイヤーの体力が1以上だったら次のターンへ遷移する
            {
                // ==============================
                // イベント呼び出し : StateMachine.PlayerChoice
                // ==============================
                SetStateMachine(BattleManager.StateMachine.State.PlayerChoice);
            }
            else // PlayerのHPが0以下になったらLoseステートに遷移する
            {
                // ==============================
                // イベント呼び出し : StateMachine.Lose
                // ==============================
                SetStateMachine(BattleManager.StateMachine.State.Lose);
            }
        }
    }
}
