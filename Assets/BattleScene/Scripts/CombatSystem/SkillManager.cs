using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// バトル中のスキル関係の制御を行うクラス
    /// </summary>
    public class SkillManager : MonoSingleton<SkillManager>
    {
        /// <summary>SkillProcessor</summary>
        public SkillJudger m_skillJudger = new SkillJudger();

        /// <summary>シャッフルスキル発動条件値</summary>
        [SerializeField] int m_shuffleSkillConditions = 30;
       
        /// <summary>PanelCounterの参照</summary>
        PanelCounter m_panelCounter;


        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            m_panelCounter = GetComponent<PanelCounter>(); // PanelCounterの参照取得
        }

        /// <summary>
        /// シャッフルスキルを発動したら、現在画面に表示されている3*3のパネルを全て非表示の状態に戻して、その3*3のパネル内でまたシャッフルさせる
        /// </summary>
        void ShufflePanels()
        {
            var shuffleCounter = m_panelCounter.m_CounterForShuffleSkill; // シャッフルスキル用のカウント数取得
            if (shuffleCounter >= m_shuffleSkillConditions) // 設定した回数以上パネルが引かれていたら
            {
                var panelManager = PanelManager.Instance; // PanelManagerの参照
                panelManager.InitPanels(); // パネルを初期化する
                m_panelCounter.ResetShuffleSkillCounter(); // カウンターをリセット
            }
        }

        /// <summary>
        /// 条件に沿ってパッシブスキルを発動させるかどうかを判断して、条件を満たすスキルの効果を反映させるイベント
        /// Skill judger.
        /// int arg0 = Level.
        /// int arg1 = City destruction count.
        /// </summary>
        public class SkillJudger : UnityEvent<SaveData.Statistics.PassiveSkill, int>
        {
            public SkillJudger() { }
        }
    }
}