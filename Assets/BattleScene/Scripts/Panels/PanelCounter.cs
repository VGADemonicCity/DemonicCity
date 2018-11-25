using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// パネルカウンタークラス
    /// パネルの種類に応じてカウントをとる
    /// 敵パネルを引いた際はStateMachineを遷移させる
    /// </summary>
    public class PanelCounter : MonoSingleton<PanelCounter>
    {
        /// <summary>シャッフルスキル専用カウンター</summary>
        public int m_CounterForShuffleSkill { get; private set; }

        /// <summary>ターン毎のパネルカウントのカウント変数</summary>
        [SerializeField] int m_CityCount;
        /// <summary>ターン毎のダブルパネルのカウント変数</summary>
        [SerializeField] int m_doubleCount;
        /// <summary>ターン毎のトリプルパネルのカウント変数</summary>
        [SerializeField] int m_tripleCount;
        /// <summary>パネルの総カウント数</summary>
        [SerializeField] int m_totalPanelCount;

        /// <summary>BattleManagerのシングルトンインスタンスの参照</summary>
        BattleManager m_battleManager;



        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            m_battleManager = BattleManager.Instance; // BattleManagerの参照取得
        }


        /// <summary>
        /// return the total count.
        /// </summary>
        /// <returns>The total count.</returns>
        public int GetPanelCount()
        {
            return m_CityCount + m_doubleCount + m_tripleCount; // そのターンパネルを引いた枚数を返す. 固有スキル用
        }

        /// <summary>
        /// そのターンの街破壊数を返す
        /// </summary>
        /// <returns>The city destruction count.</returns>
        public int GetCityDestructionCount()
        {
            return m_CityCount + (m_doubleCount * 2) + (m_tripleCount * 3); // そのターンの街破壊数を返す. パッシブスキル判定用
        }

        /// <summary>
        /// Inits the counts.
        /// </summary>
        public void InitCounts()
        {
            m_CityCount = 0;
            m_doubleCount = 0;
            m_tripleCount = 0;
            if (m_battleManager.m_state == BattleManager.StateMachine.Init) // ゲーム開始時のみトータルパネルカウントとスキル用カウンターを初期化する
            {
                m_totalPanelCount = 0;
                m_CounterForShuffleSkill = 0;
            }
        }

        /// <summary>
        /// Resets the shuffle skill flag.
        /// シャッフルスキルが使われたらこのメソッドを呼んでシャッフルスキル用カウンターを0に戻す
        /// </summary>
        public void ResetShuffleSkillCounter()
        {
            m_CounterForShuffleSkill = 0;
        }

        /// <summary>
        /// パネルのタイプで判別して対応した処理をする
        /// </summary>
        /// <param name="panel">Panel.</param>
        public void PanelJudger(Panel panel)
        {
            switch (panel.m_panelType)
            {
                case PanelType.City: // cityパネルを引いた時
                    m_CityCount++; // シティパネルカウントアップ
                    m_totalPanelCount++; // 敵を除いた全てのパネルカウントアップ
                    m_CounterForShuffleSkill++; // シャッフルスキル専用カウンターアップ
                    break;
                case PanelType.DoubleCity: // doubleパネルを引いた時
                    m_doubleCount++;// ダブルパネルカウントアップ
                    m_totalPanelCount++; // 敵を除いた全てのパネルカウントアップ
                    m_CounterForShuffleSkill++; // シャッフルスキル専用カウンターアップ
                    break;
                case PanelType.TripleCity: // tripleパネルを引いた時
                    m_tripleCount++;// トリプルパネルカウントアップ
                    m_totalPanelCount++; // 敵を除いた全てのパネルカウントアップ
                    m_CounterForShuffleSkill++; // シャッフルスキル専用カウンターアップ
                    break;
                case PanelType.Enemy: // enemyパネルを引いた時
                    m_battleManager.m_state = BattleManager.StateMachine.PlayerAttack; // ステートマシンをPlayerAttackへ
                    // =========================================
                    // イベント呼び出し : StateMachine.PlayerAttack
                    // =========================================
                    m_battleManager.m_behaviourByState.Invoke(BattleManager.StateMachine.PlayerAttack); // m_behaviourByStateイベントを起動する
                    break;
            }
        }
    }
}