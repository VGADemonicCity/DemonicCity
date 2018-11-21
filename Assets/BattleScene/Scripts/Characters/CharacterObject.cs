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

    public abstract class CharacterObject : MonoBehaviour
    {
        ///// <summary>
        ///// キャラクターのステータス.
        ///// 基本ステータス(hp,attack,defense,skillPoint)はレベルと各種ステータス(durability,muscularStrength,knowledge,sense,charm,dignity)によって決まる.
        ///// よってセーブする必要のある情報はlevel,のみ.基本ステータスはゲーム内で計算する.
        ///// </summary>
        //public class Statistics
        //{
        //    /// <summary>レベル : Character's level</summary>
        //    public int m_level;
        //    /// <summary>耐久力</summary>
        //    public int m_durability;
        //    /// <summary>筋力</summary>
        //    public int m_muscularStrength;
        //    /// <summary>知識</summary>
        //    public int m_knowledge;
        //    /// <summary>センス</summary>
        //    public int m_sense;
        //    /// <summary>魅力</summary>
        //    public int m_charm;
        //    /// <summary>威厳</summary>
        //    public int m_dignity;

        //    /// <summary>ヒットポイント</summary>
        //    public float m_hitPoint;
        //    /// <summary>攻撃力</summary>
        //    public float m_attack;
        //    /// <summary>防御力</summary>
        //    public float m_defense;
        //    /// <summary>スキルゲージポイント</summary>
        //    public float m_skillPoint;

        //}

        //public Statistics m_myStatus = new Statistics();
    }
}