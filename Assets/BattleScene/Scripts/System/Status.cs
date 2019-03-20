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
    public class Status
    {
<<<<<<< HEAD


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
=======
        #region Properties
        /// <summary>レベル : Character's level</summary>
        public int Level { get; set; }

>>>>>>> Develop
        /// <summary>耐久力</summary>
        public int Durability { get; set; }
        /// <summary>筋力</summary>
        public int MuscularStrength { get; set; }
        /// <summary>知識</summary>
        public int Knowledge { get; set; }
        /// <summary>センス</summary>
        public int Sense { get; set; }
        /// <summary>魅力</summary>
        public int Charm { get; set; }
        /// <summary>威厳</summary>
        public int Dignity { get; set; }

        /// <summary>ヒットポイント</summary>
        public int HitPoint { get; set; }
        /// <summary>攻撃力</summary>
        public int Attack { get; set; }
        /// <summary>防御力</summary>
<<<<<<< HEAD
        public int m_defense;
        /// <summary>マギアのHP最大値</summary>
        public int MaxHP
        {
            get { return Temp.m_hitPoint; }
=======
        public int Defense { get; set; }
        /// <summary>マギアのHP最大値</summary>
        public int MaxHP
        {
            get { return Temp.HitPoint; }
>>>>>>> Develop
        }

        [SerializeField] private Status m_temp;
        /// <summary>Tempにバトル開始時のhp,atk,defの初期値を一時保存しておく</summary>
        public Status Temp
        {
            get { return m_temp; }
            set { m_temp = value; }
        }
<<<<<<< HEAD

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DemonicCity.Statistics"/> class.
        /// </summary>
        public Status() { }

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


=======
        #endregion

        #region Constructors
        public Status() { }

        /// <summary>
        /// battle用のステータスのみ使う際
        /// </summary>
        /// <param name="hitPoint"></param>
        /// <param name="attack"></param>
        /// <param name="defense"></param>
        public Status(int hitPoint, int attack, int defense)
        {
            HitPoint = hitPoint;
            Attack = attack;
            Defense = defense;
        }

        public Status(int hitPoint, int attack, int defense, int level, int durability, int muscularStrength, int knowledge, int sense, int charm, int dignity)
        {
            HitPoint = hitPoint;
            Attack = attack;
            Defense = defense;
            Level = level;
            Durability = durability;
            MuscularStrength = muscularStrength;
            Knowledge = knowledge;
            Sense = sense;
            Charm = charm;
            Dignity = dignity;
        }
        #endregion

        #region OperatorOverLoads
        /// <summary>
        /// ２つのStatusクラスを合算したStatusを返す
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Status operator +(Status a, Status b)
        {
            return new Status
            {
                Level = a.Level + b.Level,
                HitPoint = a.HitPoint + b.HitPoint,
                Attack = a.Attack + b.Attack,
                Defense = a.Defense + b.Defense,
                Durability = a.Durability + b.Durability,
                MuscularStrength = a.MuscularStrength + b.MuscularStrength,
                Knowledge = a.Knowledge + b.Knowledge,
                Sense = a.Sense + b.Sense,
                Charm = a.Charm + b.Charm,
                Dignity = a.Dignity + b.Dignity
            };
        }

        /// <summary>
        /// ２つのStatusの各メンバーの差分をStatusで返す
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Status operator -(Status a, Status b)
        {
            return new Status
            {
                Level = a.Level - b.Level,
                HitPoint = a.HitPoint - b.HitPoint,
                Attack = a.Attack - b.Attack,
                Defense = a.Defense - b.Defense,
                Durability = a.Durability - b.Durability,
                MuscularStrength = a.MuscularStrength - b.MuscularStrength,
                Knowledge = a.Knowledge - b.Knowledge,
                Sense = a.Sense - b.Sense,
                Charm = a.Charm - b.Charm,
                Dignity = a.Dignity - b.Dignity,
            };
        }
        #endregion

        #region Methods
>>>>>>> Develop
        /// <summary>
        /// バトル開始時のステータスの初期値を保存
        /// </summary>
        /// <param name="stats">Stats.</param>
        public void Init(Status stats)
        {
            Temp = new Status
            {
<<<<<<< HEAD
                m_hitPoint = stats.m_hitPoint,
                m_attack = stats.m_attack,
                m_defense = stats.m_defense
            };
        }
=======
                HitPoint = stats.HitPoint,
                Attack = stats.Attack,
                Defense = stats.Defense,
            };
        }
        #endregion
>>>>>>> Develop
    }
}