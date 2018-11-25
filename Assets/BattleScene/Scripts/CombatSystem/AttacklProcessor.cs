using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Attackl processor.
    /// 攻撃処理を行うクラス
    /// </summary>
    public class AttacklProcessor : MonoSingleton<AttacklProcessor>
    {
        /// <summary>BattleManagerのシングルトンインスタンスの参照</summary>
        BattleManager m_battleManager;
        /// <summary>PanelCounterの参照</summary>
        PanelCounter m_panelCounter;
        /// <summary>SkillManagerの参照</summary>
        protected SkillManager m_skillManager;
        /// <summary>Magiaの参照</summary>
        Magia m_magia;

        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            m_battleManager = BattleManager.Instance; // BattleManagerの参照取得
            m_skillManager = SkillManager.Instance; // SkillManagerの参照取得
            m_panelCounter = PanelCounter.Instance; // PanelCounterの参照取得
            m_magia = Magia.Instance; // Magiaの参照取得
        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_behaviourByState.AddListener((state) => // ステートマシンにイベント登録
            {
                if(state != BattleManager.StateMachine.PlayerAttack) // StateがPlayerAttack以外の時は処理終了
                {
                    return;
                }
                // ==============================
                // イベント呼び出し
                // ==============================
                m_skillManager.m_skillJudger.Invoke(m_magia.m_stats.m_passiveSkill, m_panelCounter.GetCityDestructionCount()); // SkillManagerのイベントを呼び出してPassiveSkillをステータスに反映させる
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
            // ここに攻撃の演出処理を入れる予定かな？
            // ==============================
            Debug.Log("アタックプロセス呼ばれた");
            yield return new WaitForSeconds(3f);

            yield return new WaitWhile(() => // falseになるまで待つ
            {
                Debug.Log("State change to EnemyChoice");
                m_battleManager.m_state = BattleManager.StateMachine.Judge; // 敵が残存しているかどうかの判定ステートに遷移する
                return false;
            });
        }
    }
}
