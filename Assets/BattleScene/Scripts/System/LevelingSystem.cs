using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    public class LevelingSystem<T> : MonoBehaviour where T : Magia
    {
        /// <summary>Magia</summary>
        Magia m_magia;
        /// <summary>振り分けポイント</summary>
        int m_allocationPoint;
        /// <summary>LevelUp毎の固有ステータス用振り分けポイント増加量</summary>
        int m_addStatsPoint;

        private void Awake()
        {
            m_magia = Magia.Instance;

        }

        /// <summary>
        /// レベル上限に達していない、且つ次のレベルに上がるのに必要な経験値を超えていたら
        /// 1レベルアップする.
        /// </summary>
        public void LevelUp(T target, Statistics stats)
        {
            var requiredExp = m_magia.GetRequiredExpToNextLevel(m_magia.Stats.m_level); // 現在のレベルに必要な経験値(総パネル破壊枚数)

            if (m_magia.MaxLevel >= m_magia.Stats.m_level && requiredExp <= m_magia.Stats.m_level) // レベル上限を越していない且つ必要経験値以上の経験値を取得している　
            {
                return;
            }

            // レベルアップする直前のレベルに合わせてステータスを上昇させる
            if (m_magia.Stats.m_level < 50) // レベル50以下なら
            {
                m_magia.Stats.m_hitPoint += 50;
                m_magia.Stats.m_attack += 15;
                m_magia.Stats.m_defense += 15;
            }
            else if (m_magia.Stats.m_level >= 50 && m_magia.Stats.m_level < 100) // レベル50~99なら
            {
                m_magia.Stats.m_hitPoint += 25;
                m_magia.Stats.m_attack += 10;
                m_magia.Stats.m_defense += 10;
            }
            else if (m_magia.Stats.m_level >= 100 && m_magia.Stats.m_level < 150) // レベル100~149なら
            {
                m_magia.Stats.m_hitPoint += 10;
                m_magia.Stats.m_attack += 5;
                m_magia.Stats.m_defense += 5;
            }
            else if (m_magia.Stats.m_level >= 150 && m_magia.Stats.m_level < 200) // レベル150~199なら
            {
                m_magia.Stats.m_hitPoint += 5;
                m_magia.Stats.m_attack += 1;
                m_magia.Stats.m_defense += 1;
            }

            m_magia.Stats.m_level++; // levelを1上げる
            m_allocationPoint += m_addStatsPoint; // レベルが上がる毎にステータスに振り分ける事が可能なポイントを一定値渡す
        }

    }
}
