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
    public struct Status
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
        }

        [SerializeField] private Status m_temp;
        /// <summary>Tempにバトル開始時のhp,atk,defの初期値を一時保存しておく</summary>
        public Status Temp
        {
            get { return m_temp; }
            set { m_temp = value; }
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="T:DemonicCity.Statistics"/> class.
        /// </summary>
        /// <param name="hitPoint">Hit point.</param>
        /// <param name="attack">Attack.</param>
        /// <param name="defense">Defense.</param>
        public Status(int hitPoint, int attack, int defense)
        {
            m_hitPoint = hitPoint;
            m_attack = attack;
            m_defense = defense;
        }


        /// <summary>
        /// バトル開始時のステータスの初期値を保存
        /// </summary>
        /// <param name="stats">Stats.</param>
        public void Init(Status stats)
        {
            Temp = new Status
            {
                m_hitPoint = stats.m_hitPoint,
                m_attack = stats.m_attack,
                m_defense = stats.m_defense
            };
        }
    }
}
