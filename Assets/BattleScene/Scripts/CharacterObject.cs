using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    public abstract class CharacterObject : MonoBehaviour
    {
        /// <summary>ヒットポイント</summary>
        [SerializeField] float m_hitPoint = 0;
        /// <summary>攻撃力</summary>
        [SerializeField] float m_attack = 0;
        /// <summary>防御力</summary>
        [SerializeField] float m_defense = 0;
        /// <summary>スキルゲージポイント</summary>
        [SerializeField] float m_skillPoint = 0;

        protected virtual void Start()
        {

        }

        /// <summary>
        /// 各引数とlevelに応じてステータスを決める
        /// </summary>
        /// <param name="level">Level.</param>
        /// <param name="hp">Hp.</param>
        /// <param name="attack">Attack.</param>
        /// <param name="defense">Defense.</param>
        protected virtual void SetStatus(int level,float hp,float attack,float defense)
        {
            m_hitPoint = hp;
            m_attack = attack;
            m_defense = defense;
        }

    }
}