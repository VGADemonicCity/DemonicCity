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
        #region Properties
        /// <summary>レベル : Character's level</summary>
        public int Level;

        /// <summary>耐久力</summary>
        public int Durability;
        /// <summary>筋力</summary>
        public int MuscularStrength;
        /// <summary>知識</summary>
        public int Knowledge;
        /// <summary>センス</summary>
        public int Sense;
        /// <summary>魅力</summary>
        public int Charm;
        /// <summary>威厳</summary>
        public int Dignity;

        /// <summary>ヒットポイント</summary>
        public int HitPoint;
        /// <summary>攻撃力</summary>
        public int Attack;
        /// <summary>防御力</summary>
        public int Defense;
        /// <summary>マギアのHP最大値</summary>
        public int MaxHP
        {
            get { return Temp.HitPoint; }
        }

        [SerializeField] private Status m_temp;
        /// <summary>Tempにバトル開始時のhp,atk,defの初期値を一時保存しておく</summary>
        public Status Temp
        {
            get { return m_temp; }
            set { m_temp = value; }
        }
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
        /// <summary>
        /// バトル開始時のステータスの初期値を保存
        /// </summary>
        /// <param name="stats">Stats.</param>
        public void Init(Status stats)
        {
            Temp = new Status
            {
                HitPoint = stats.HitPoint,
                Attack = stats.Attack,
                Defense = stats.Defense,
            };
        }
        #endregion
    }
}
