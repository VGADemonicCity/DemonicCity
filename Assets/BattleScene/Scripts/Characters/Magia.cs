using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

namespace DemonicCity
{
    /// <summary>
    /// Magia.
    /// </summary>
    [Serializable]
    public class Magia : SSB<Magia>
    {
        /// <summary>属性</summary>
        [Serializable]
        public enum Attribute
        {
            /// <summary>初期形態</summary>
            Standard,
            /// <summary>男近接形態</summary>
            MaleWarrior,
            /// <summary>女近接形態</summary>
            FemaleWarrior,
            /// <summary>男魔法使い形態</summary>
            MaleWizard,
            /// <summary>女魔法使い形態</summary>
            FemaleWitch,
            /// <summary>女超越形態</summary>
            FemaleTrancendental,
        }

        /// <summary>
        /// レベルアップ獲得スキル。
        /// レベルが一定値上がったら対応したスキルが解放されて、以降永続的に使用可能となる。
        /// </summary>
        [Flags, Serializable]
        public enum PassiveSkill
        {
            /// <summary>無効値</summary>
            Invalid = 0,
            /// <summary>魔拳</summary>
            DevilsFist = 1,
            /// <summary>高濃度魔力吸収,High concentration magical absorption</summary>
            HighConcentrationMagicalAbsorption = 2,
            /// <summary>自己再生</summary>
            SelfRegeneration = 4,
            /// <summary>爆炎熱風柱</summary>
            ExplosiveFlamePillar = 8,
            /// <summary>紅蓮障壁</summary>
            CrimsonBarrier = 16,
            /// <summary>魔拳烈火ノ型</summary>
            DevilsFistInfernoType = 32,
            /// <summary>心焔権現</summary>
            BraveHeartsIncarnation = 64,
            /// <summary>大紅蓮障壁</summary>
            GreatCrimsonBarrier = 128,
            /// <summary>豪炎爆砕掌</summary>
            InfernosFist = 256,
            /// <summary>魔王ノ細胞</summary>
            SatansCell = 512,
            AmaterasuIncanation = 1024,
            /// <summary>天照-爆炎-</summary>
            AmaterasuInferno = 2048,
            /// <summary>天照-焔壁-</summary>
            AmaterasuFlameWall = 4096,
            /// <summary>全てのスキルフラグ(全てのenumの論理和)</summary>
            AllSkills = 8191
        }

        /// <summary>経験値</summary>
        [SerializeField] int m_totalExperience;
        /// <summary>振り分けポイント</summary>
        [SerializeField] float m_statsPoint;
        /// <summary>属性フラグ</summary>
        [SerializeField] Attribute m_attribute = Attribute.Standard;
        /// <summary>レベル毎の必要経験値数</summary>
        [SerializeField] int[] requiredExps = { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 60, 70, 80, 90, 100, 150, 200, 250, 300, 400, 500 };
        /// <summary>パッシブスキルフラグ</summary>     
        [SerializeField] PassiveSkill m_passiveSkill = PassiveSkill.AllSkills;
        /// <summary>パッシブスキルフラグのプロパティ</summary>
        public PassiveSkill MyPassiveSkill
        {
            get { return m_passiveSkill; }
        }
        /// <summary>初期レベルを1としたときの最大レベルを返します</summary>
        /// <value>レベル最大値</value>
        public int MaxLevel { get { return requiredExps.Length + 1; } }
        /// <summary>レベルアップに必要な経験値(破壊したパネルの総数)</summary>
        /// <summary>実際にセーブするステータスクラス</summary>
        [SerializeField]
        Statistics m_stats = new Statistics()
        {
            m_level = 1,
            m_hitPoint = 1000,
            m_attack = 100,
            m_defense = 100,
            m_charm = 0,
            m_sense = 0,
            m_dignity = 0,
            m_knowledge = 0,
            m_durability = 0,
            m_muscularStrength = 0
        };
        /// <summary>ステータスクラスのプロパティ</summary>
        public Statistics Stats
        {
            get { return m_stats; }
            set { m_stats = value; }
        }
        /// <summary>レベルアップ時に得れるステータスポイント</summary>
        public float m_statusPoint;

        /// <summary>マギアのステータスを一時保存しておく変数</summary>
        [SerializeField] Statistics m_StatsBuffer;
        /// <summary>m_statsBufferのプロパティ</summary>
        public Statistics StatsBuffer
        {
            get { return m_StatsBuffer; }
            set { m_StatsBuffer = value; }
        }

        /// <summary>固有ステータス用振り分けポイント</summary>
        int m_addStatsPoint = 3;
        /// <summary>固有ステータスを基礎ステータスに変換する際の倍率</summary>
        int m_magnificationByStats = 5;
        /// <summary>固有ステータスを形態毎に基礎ステータスに変換する際の倍率</summary>
        int m_magnificationByAttribute = 50;

        /// <summary>
        /// 次のレベルに上がるために必要な経験値を返します
        /// </summary>
        /// <returns>The required exp to next level.</returns>
        /// <param name="currentLevel">Current level.</param>
        public int GetRequiredExpToNextLevel(int currentLevel)
        {
            return currentLevel >= MaxLevel ? 0 : requiredExps[currentLevel - 1];
        }

        /// <summary>
        /// レベル上限に達していない、且つ次のレベルに上がるのに必要な経験値を超えていたら
        /// 1レベルアップする.
        /// </summary>
        public void LevelUp()
        {
            var requiredExp = GetRequiredExpToNextLevel(Stats.m_level); // 現在のレベルに必要な経験値(総パネル破壊枚数)

            if (MaxLevel >= Stats.m_level && requiredExp <= Stats.m_level) // レベル上限を越していない且つ必要経験値以上の経験値を取得している　
            {
                return;
            }
            Stats.m_level++; // levelを1上げる
            m_statsPoint += m_addStatsPoint; // レベルが上がる毎にステータスに振り分ける事が可能なポイントを一定値渡す
        }

        /// <summary>
        /// 初期形態のレベル1のステータスにセットする
        /// </summary>
        public void ResetStats()
        {
            Stats = new Statistics()
            {
                m_level = 1,
                m_hitPoint = 1000,
                m_attack = 100,
                m_defense = 100,
                m_charm = 0,
                m_sense = 0,
                m_dignity = 0,
                m_knowledge = 0,
                m_durability = 0,
                m_muscularStrength = 0
            };
        }

        /// <summary>
        /// 強化画面で編集したStatsをmagiaにセットし、固有ステータスを基礎ステータスに反映させる
        /// </summary>
        public void SetStats(Statistics stats = null)
        {
            if (stats != null)
            {
                Stats = stats;
            }
            Stats.m_attack = Stats.m_attack + (Stats.m_sense * m_magnificationByStats); // センスを攻撃力に変換
            Stats.m_attack = Stats.m_attack + (Stats.m_muscularStrength * m_magnificationByStats); // 筋力を攻撃力に変換
            Stats.m_defense = Stats.m_defense + (Stats.m_durability * m_magnificationByStats); // 耐久力を防御力に変換
            Stats.m_defense = Stats.m_defense + (Stats.m_knowledge * m_magnificationByStats); // 知識を防御力に変換
            Stats.m_hitPoint = Stats.m_hitPoint + (Stats.m_charm * m_magnificationByAttribute); // 魅力をHPに変換
            Stats.m_hitPoint = Stats.m_hitPoint + (Stats.m_dignity * m_magnificationByAttribute); // 威厳をHPに変換
        }
    }
}
