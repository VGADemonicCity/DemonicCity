using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Skill processor.
    /// </summary>
    public class SkillProcessor : MonoBehaviour
    {
        /// <summary>BattleManagerのシングルトンインスタンスの参照</summary>
        BattleManager m_battleManager;
        PanelCounter m_panelCounter;


        void Awake()
        {
            m_battleManager = BattleManager.Instance; // BattleManagerの参照取得
        }
        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_behaviourByState.AddListener((states) => // ステートマシンにイベント登録
            {
                if(states != BattleManager.States.PlayerAttack) // StateがPlayerAttack以外の時は処理終了
                {
                    return;
                }
                Debug.Log("invoked");
                StartCoroutine(AttackProcess()); // 攻撃プロセス
            });
        }

        /// <summary>
        /// Attacks the process.
        /// </summary>
        /// <returns>The process.</returns>
        IEnumerator AttackProcess()
        {
            Debug.Log("アタックプロセス呼ばれた");
            yield return new WaitForSeconds(3f);

            yield return new WaitWhile(() => // falseが変えるまで待つ
            {
                Debug.Log("State change to EnemyChoice");
                m_battleManager.m_states = BattleManager.States.EnemyChoice; // 敵の攻撃ステートに遷移する
                return false;
            });
        }
    }
}
