using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    /// <summary>
    /// キャラクターのステータス
    /// </summary>
    [Serializable]
    public class Statistics
    {


        ///// <summary>
        ///// 固有スキル。
        ///// プレイアブルキャラクターの形態に応じて使用可能スキルが変わる
        ///// </summary>
        //[Flags]
        //public enum UniqueSkill
        //{
        //    EvilEye = 1,
        //    AppearanceOfDestruction = 2,
        //    QueensBreath = 4,
        //    KillersSword = 8,
        //    DarkVibration = 16,
        //    OmniscientAbility = 32,
        //    OmniscentAndOmnipotent = 64
        //}


        /// <summary>レベル : Character's level</summary>
        public int m_level;
        /// <summary>耐久力</summary>
        public int m_durability;
        /// <summary>筋力</summary>
        public int m_muscularStrength;
        /// <summary>知識</summary>
        public int m_knowledge;
        /// <summary>センス</summary>
        public int m_sense;
        /// <summary>魅力</summary>
        public int m_charm;
        /// <summary>威厳</summary>
        public int m_dignity;

        /// <summary>ヒットポイント</summary>
        public int m_hitPoint;
        /// <summary>攻撃力</summary>
        public int m_attack;
        /// <summary>防御力</summary>
        public int m_defense;
        /// <summary>マギアのHP最大値</summary>
        public int MaxHP
        {
            get { return Temp.m_hitPoint; }
            private set { MaxHP = value; }
        }

        [SerializeField] private Statistics m_temp;
        /// <summary>Tempにhp,atk,defを保存しておく</summary>
        public Statistics Temp
        {
            get { return m_temp; }
            set { m_temp = value; }
        }
        /// <summary>
        /// 基礎ステータスをTempに保存した時の状態に戻す
        /// </summary>
        public void Reset()
        {
            m_attack = Temp.m_attack;
            m_defense = Temp.m_defense;
        }

        /// <summary>
        /// バトル開始時のステータスの初期値を保存
        /// </summary>
        /// <param name="stats">Stats.</param>
        public void Init(Statistics stats)
        {
            Temp = new Statistics
            {
                m_hitPoint = stats.m_hitPoint,
                m_attack = stats.m_attack,
                m_defense = stats.m_defense
            };
        }
    }
}
