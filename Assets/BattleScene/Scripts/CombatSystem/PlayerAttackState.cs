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
        /// <summary>MagiaのHPDrawの参照</summary>
        [SerializeField] protected HPDraw m_magiaHpDraw;
        /// <summary>EnemyのHPDrawの参照</summary>
        [SerializeField] protected HPDraw m_enemyHpDraw;

        protected override void Awake()
        {
            base.Awake();
            m_magiaHpDraw = m_magiaHpDraw.GetComponent<HPDraw>(); // magiaのHPDrawコンポーネント取得
            m_enemyHpDraw = m_enemyHpDraw.GetComponent<HPDraw>(); // enemyのHPDrawコンポーネント取得
        }


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


            Debug.Log("攻撃する前の[" + m_battleManager.m_enemy.Id.ToString() + "]の体力 : " + m_battleManager.m_enemy.Stats.m_hitPoint);
            yield return new WaitForSeconds(3f);

            yield return new WaitWhile(() => // falseになるまで待つ
            {
                Debug.Log("PlayerAttack state called.");
                var damage = m_magia.Stats.m_attack - m_battleManager.m_enemy.Stats.m_defense;
                m_battleManager.m_enemy.Stats.m_hitPoint -= damage; // プレイヤーの攻撃力から敵防御力を引いた値分ダメージ
                m_enemyHpDraw.Damage(damage); // HPGaugeに描画
                Debug.Log("攻撃した後の[" + m_battleManager.m_enemy.Id.ToString() + "]の体力 : " + m_battleManager.m_enemy.Stats.m_hitPoint);


                return false;
            });


            // ==================================
            // イベント呼び出し : StateMachine.
            // ==================================
            if (m_battleManager.m_enemy.Stats.m_hitPoint > 0) // 敵のHPが1以上だったら敵の攻撃ステートに遷移
            {
                // ==================================
                // イベント呼び出し : StateMachine.EnemyAttack
                // ==================================
                SetStateMachine(BattleManager.StateMachine.State.EnemyAttack);
            }
            else // 敵のHPが0以下だったら
            {
                // ==================================
                // イベント呼び出し : StateMachine.Win
                // もし次のWaveが存在すれば、次のWaveへ遷移する処理を書く
                // ==================================
                SetStateMachine(BattleManager.StateMachine.State.Win);
            }
        }
    }
}
