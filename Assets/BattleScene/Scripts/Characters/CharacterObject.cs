using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    /// <summary>
    /// SingleTon.
    /// Character object.
    /// 全てのキャラクタークラスの基底クラス.
    /// </summary>
    public abstract class CharacterObject : MonoSingleton<CharacterObject>
    {
        /// <summary>
        /// キャラクターのステータス.
        /// 基本ステータス(hp,attack,defense,skillPoint)はレベルと各種ステータス(durability,muscularStrength,knowledge,sense,charm,dignity)によって決まる.
        /// よってセーブする必要のある情報はlevel,のみ.基本ステータスはゲーム内で計算する.
        /// </summary>
        public class Statistics
        {
            /// <summary>レベル : Character's level</summary>
            public int m_level { get; private set; }


            /// <summary>耐久力</summary>
            public int m_durability { get; private set; }
            /// <summary>筋力</summary>
            public int m_muscularStrength { get; private set; }
            /// <summary>知識</summary>
            public int m_knowledge { get; private set; }
            /// <summary>センス</summary>
            public int m_sense { get; private set; }
            /// <summary>魅力</summary>
            public int m_charm { get; private set; }
            /// <summary>威厳</summary>
            public int m_dignity { get; private set; }

            /// <summary>ヒットポイント</summary>
            public float m_hitPoint { get; private set; }
            /// <summary>攻撃力</summary>
            public float m_attack { get; private set; }
            /// <summary>防御力</summary>
            public float m_defense { get; private set; }
            /// <summary>スキルゲージポイント</summary>
            public float m_skillPoint { get; private set; }

        }




        public Statistics m_myStatus = new Statistics();
    }
}