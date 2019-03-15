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
    }
}
