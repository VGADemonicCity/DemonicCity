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
        /// <summary>m_counterFoeShuffleSkillのプロパティ</summary>
        public int CounterForShuffleSkill
        {
            get
            {
                return m_counterForShuffleSkill;
            }
            private set
            {
                m_counterForShuffleSkill = value;
            }
        }

        /// <summary>
        /// Gets the destruction count.
        /// </summary>
        /// <value>The destruction count.</value>
        public int DestructionCount
        {
            get
            {
                return m_destructionCount;
            }
        }

        /// <summary>
        /// Gets the total destruction count.
        /// </summary>
        /// <value>The total destruction count.</value>
        public int TotalDestructionCount
        {
            get
            {
                return m_totalDestructionCount;
            }
            set
            {
                m_totalDestructionCount = value;
            }
        }

        /// <summary>シャッフルスキル専用カウンター : inspectorに表示させる為アクセサーと使用変数を分けている</summary>
        [SerializeField] int m_counterForShuffleSkill;
        /// <summary>ターン毎のパネルカウントのカウント変数</summary>
        [SerializeField] int m_CityCount;
        /// <summary>ターン毎のダブルパネルのカウント変数</summary>
        [SerializeField] int m_doubleCount;
        /// <summary>そのターンの街破壊数</summary>
        [SerializeField] int m_destructionCount;
        /// <summary>ターン毎のトリプルパネルのカウント変数</summary>
        [SerializeField] int m_tripleCount;
        /// <summary>そのバトルにおけるパネルの総カウント数</summary>
        [SerializeField] int m_totalPanelCount;
        /// <summary>そのバトルにおける総街破壊数</summary>
        [SerializeField] int m_totalDestructionCount;
        /// <summary>固有スキルゲージ</summary>
        [SerializeField] UniqueSkillGauge m_uniqueSkillGauge;

        /// <summary>BattleManagerのシングルトンインスタンスの参照</summary>
        BattleManager m_battleManager;



        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            m_battleManager = BattleManager.Instance; // BattleManagerの参照取得
            if (m_battleManager == null) Debug.LogError("BattleManager is null.");
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
        /// Inits the counts.
        /// </summary>
        public void InitCounts()
        {
            m_CityCount = 0;
            m_doubleCount = 0;
            m_tripleCount = 0;
            m_destructionCount = 0;
            if (m_battleManager.m_StateMachine.m_State == BattleManager.StateMachine.State.Init) // ゲーム開始時のみトータルパネルカウントとスキル用カウンターを初期化する
            {
                m_totalPanelCount = 0;
                m_counterForShuffleSkill = 0;
                m_totalDestructionCount = 0;
            }
        }

        /// <summary>
        /// Resets the shuffle skill flag.
        /// シャッフルスキルが使われたらこのメソッドを呼んでシャッフルスキル用カウンターを0に戻す
        /// </summary>
        public void ResetShuffleSkillCounter()
        {
            m_counterForShuffleSkill = 0;
        }

        /// <summary>
        /// パネルのタイプで判別して対応した処理をする
        /// </summary>
        /// <param name="panel">Panel.</param>
        public void PanelJudger(Panel panel)
        {
            var destructionCount = 0; // 街破壊数の一時保存変数
            switch (panel.MyPanelType)
            {
                case PanelType.City: // cityパネルを引いた時
                    m_CityCount++; // シティパネルカウントアップ
                    destructionCount++;
                    break;
                case PanelType.DoubleCity: // doubleパネルを引いた時
                    m_doubleCount++;// ダブルパネルカウントアップ
                    destructionCount += 2;
                    break;
                case PanelType.TripleCity: // tripleパネルを引いた時
                    m_tripleCount++;// トリプルパネルカウントアップ
                    destructionCount += 3;
                    break;
                case PanelType.Enemy: // enemyパネルを引いた時
                    m_battleManager.m_StateMachine.m_State = BattleManager.StateMachine.State.PlayerAttack; // ステートマシンをPlayerAttackへ
                    // =========================================
                    // イベント呼び出し : StateMachine.PlayerAttack
                    // =========================================
                    m_battleManager.m_BehaviourByState.Invoke(BattleManager.StateMachine.State.PlayerAttack); // m_behaviourByStateイベントを起動する
                    break;
            }
            m_totalPanelCount++; // 全てのパネルカウントアップ
            m_counterForShuffleSkill++; // シャッフルスキル専用カウンターアップ
            m_totalDestructionCount += destructionCount; // 総計に加算
            m_destructionCount += destructionCount;
            m_uniqueSkillGauge.Sync(); // 固有スキルゲージに同期
        }
    }
}