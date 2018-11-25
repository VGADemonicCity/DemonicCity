using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Judge manager.
    /// プレイヤー,敵の生存確認を行い、StateMachineの遷移先を決定するクラス
    /// </summary>
    public class JudgeManager : MonoSingleton<JudgeManager>
    {
        /// <summary>BattleManagerの参照</summary>
        BattleManager m_battleManager;
        /// <summary>Magiaの参照</summary>
        Magia m_magia;

        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            m_battleManager = BattleManager.Instance; // BattleManagerの参照取得
            m_magia = Magia.Instance; // Magiaの参照取得
        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_behaviourByState.AddListener((state) => // BattleManagerにイベントを登録する
            {
                if(m_battleManager.m_state != BattleManager.StateMachine.Judge) // StateがJudge以外の時は処理終了
                {
                    return;
                }
                // ==============================
                // ここに敵と味方の生存確認処理を書く
                // ==============================
                if(true)
                {
                    m_battleManager.m_state = BattleManager.StateMachine.NextTurn; // stateMachineをNextTurnへ遷移させる
                    // ==============================
                    // イベント呼び出し : NextTurn
                    // ==============================
                    m_battleManager.m_behaviourByState.Invoke(BattleManager.StateMachine.NextTurn);
                }
                else 
                {
                    // ==============================
                    // 分岐処理を実装予定(elseでは書かない？)
                    // ==============================
                }

            });
        }
    }
}
