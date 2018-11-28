using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DemonicCity
{
    /// <summary>
    /// Magia.
    /// </summary>
    [Serializable]
    public class Magia : SavableSingletonBase<Magia>
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
            /// <summary>天照-爆炎-</summary>
            AmaterasuInferno = 1024,
            /// <summary>天照-焔壁-</summary>
            AmaterasuFlameWall = 2048,
            /// <summary>全てのスキルフラグ(全てのenumの論理和)</summary>
            AllSkills = 4095
        }

        /// <summary>経験値</summary>
        public int m_totalExperience;
        /// <summary>属性フラグ</summary>
        public Attribute m_attribute = Attribute.Standard;
        /// <summary>パッシブスキルフラグ</summary>
        public PassiveSkill m_passiveSkill = PassiveSkill.AllSkills;
        /// <summary>
        /// 初期レベルを1としたときの最大レベルを返します
        /// </summary>
        /// <value>The max level.</value>
        public int MaxLevel { get { return requiredExps.Length + 1; } }
        /// <summary>レベルアップに必要な経験値</summary>
        [SerializeField]
        int[] requiredExps = { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 60, 70, 80, 90, 100, 150, 200, 250, 300, 400, 500 };
        /// <summary>実際にセーブするステータスクラス</summary>
        [SerializeField] Statistics m_stats = new Statistics();
        /// <summary>ステータスクラスのプロパティ</summary>
        public Statistics Stats
        {
            get { return m_stats; }
            set { m_stats = value; }
        }

        /// <summary>
        /// レベル上限に達していない、且つ次のレベルに上がるのに必要な経験値を超えていたら
        /// 1レベルアップする.
        /// </summary>
        public void LevelUp()
        {
            var requiredExp = GetRequiredExpToNextLevel(Stats.m_level);

            if (MaxLevel >= Stats.m_level && requiredExp <= Stats.m_level)
            {
                return;
            }
            Stats.m_level++;
        }

        /// <summary>
        /// 次のレベルに上がるために必要な経験値を返します
        /// </summary>
        /// <returns>The required exp to next level.</returns>
        /// <param name="currentLevel">Current level.</param>
        public int GetRequiredExpToNextLevel(int currentLevel)
        {
            return currentLevel >= MaxLevel ? 0 : requiredExps[currentLevel - 1];
        }
    }
}
