using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// 
    /// </summary>
    public class UniqueSkillManager : MonoSingleton<UniqueSkillManager>
    {
        /// <summary>ユニークスキルが使用可能かどうか判断するフラグ</summary>
        public bool SkillFlag
        {
            get { return m_flag; }
            set { m_flag = value; }
        }
        /// <summary>固有スキル発動条件(開いたパネル枚数)</summary>
        public int Condition
        {
            get { return m_condition; }
            set { m_condition = value; }
        }

        /// <summary>UniqueSkillGaugeの参照</summary>
        [SerializeField] UniqueSkillGauge m_uniqueSkillGauge;
        /// <summary>固有スキル発動条件(開いたパネル枚数)</summary>
        [SerializeField] int m_condition;
        /// <summary>ユニークスキルが使用可能かどうか判断するフラグ</summary>
        [SerializeField] bool m_flag;
        /// <summary>スキル使用時の確認画面</summary>
        [SerializeField] ConfirmWindow m_confirmWindow;

        /// <summary>TouchGestureDetectorの参照</summary>
        TouchGestureDetector m_touchGestureDetector;
        /// <summary>BattleManagerの参照</summary>
        BattleManager m_battleManager;
        /// <summary>Magiaの参照</summary>
        Magia m_magia;

        private void Awake()
        {
            m_touchGestureDetector = TouchGestureDetector.Instance; // shingleton,TouchGestureDetectorインスタンスの取得
            m_battleManager = BattleManager.Instance; // shingleton,BattleManagerインスタンスの取得
            m_magia = Magia.Instance; // 参照取得

            m_battleManager.m_BehaviourByState.AddListener((state) =>
            {
                // init時マギアの形態に応じてユニークスキルの条件値を設定する
                if (state == BattleManager.StateMachine.State.Init)
                {
                    Condition = m_magia.UniqueSkillConditionByAttribute;
                }
            });
        }

        private void Start()
        {
            m_touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                GameObject hitResult;
                touchInfo.HitDetection(out hitResult);

                // クリック時 && プレイヤー選択時 
                // && ユニークスキルフラグtrueの時
                //&& タッチしたゲームオブジェクトのタグが"PlayerSkillGauge"の時
                if (gesture == TouchGestureDetector.Gesture.Click
                && m_battleManager.m_StateMachine.m_State == BattleManager.StateMachine.State.PlayerChoice
                && SkillFlag == true
                && hitResult != null)
                {
                    if (hitResult.tag != "PlayerSkillGauge")
                    {
                        return;
                    }
                    m_popupSystem.gameObject.SetActive(true);
                    m_popupSystem.Popup();
                }
            });
        }

        /// <summary>
        /// 固有スキル発動
        /// </summary>
        public void Activate()
        {
            // 形態に応じたユニークスキルを取得,発動する.
            var uniqueSkillFactory = GetComponent<UniqueSkillFactory>();
            var uniqueSkill = uniqueSkillFactory.Create(m_magia.MyAttribute);
            uniqueSkill.Activate();
            m_uniqueSkillGauge.SkillActivated();
        }
    }
}
