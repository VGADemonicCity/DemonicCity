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
    public class Magia : SavableSingletonBase<Magia>
    {

        /// <summary>パッシブスキルフラグのプロパティ</summary>
        public PassiveSkill MyPassiveSkill
        {
            get { return m_passiveSkill; }
        }
        /// <summary>初期レベルを1としたときの最大レベルを返す</summary>
        public int MaxLevel
        {
            get { return m_requiredExps.Length + 1; }
        }
        /// <summary>ステータスクラスのプロパティ</summary>
        public Statistics Stats
        {
            get { return m_stats; }
            set { m_stats = value; }
        }
        /// <summary>振り分けポイントのプロパティ</summary>
        public int AllocationPoint
        {
            get { return m_allocationPoint; }
            set { m_allocationPoint = value; }
        }
        /// <summary>m_attributeのプロパティ</summary>
        public Attribute MyAttribute
        {
            get { return m_attribute; }
            set { m_attribute = value; }
        }

        /// <summary>m_totalExperienceのプロパティ</summary>
        public int TotalExperience
        {
            get { return m_totalExperience; }
            set { m_totalExperience = value; }
        }

        /// <summary>m_totalDestructionCountのプロパティ</summary>
        public int TotalDestructionCount
        {
            get { return m_totalDestructionCount; }
            set { m_totalDestructionCount = value; }
        }

        /// <summary>マギアのHP最大値</summary>
        public int MaxHP { get; private set; }

        /// <summary>属性別ユニークスキルの条件値を返す</summary>
        public int UniqueSkillConditionByAttribute
        {
            get
            {
                int condition;
                switch (MyAttribute)
                {
                    case Attribute.Standard:
                        condition = 30;
                        break;
                    case Attribute.MaleWarrior:
                        condition = 30;
                        break;
                    case Attribute.FemaleWarrior:
                        condition = 30;
                        break;
                    case Attribute.MaleWizard:
                        condition = 30;
                        break;
                    case Attribute.FemaleWitch:
                        condition = 30;
                        break;
                    case Attribute.FemaleTrancendental:
                        condition = 30;
                        break;
                    default:
                        condition = 30;
                        Debug.LogError("属性が設定されていません。");
                        break;
                }
                return condition;
            }
        }


        /// <summary>経験値</summary>
        [SerializeField] int m_totalExperience;
        /// <summary>総街破壊数</summary>
        [SerializeField] int m_totalDestructionCount;
        /// <summary>振り分けポイント</summary>
        [SerializeField] int m_allocationPoint;
        /// <summary>属性フラグ</summary>
        [SerializeField] Attribute m_attribute = Attribute.Standard;
        /// <summary>レベルアップに必要な経験値(破壊したパネルの総数)</summary>
        [SerializeField] int[] m_requiredExps = { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 60, 70, 80, 90, 100, 150, 200, 250, 300, 400, 500 };
        /// <summary>パッシブスキルフラグ</summary>     
        [SerializeField] PassiveSkill m_passiveSkill = PassiveSkill.AllSkill;

        /// <summary>実際にセーブするステータスクラス</summary>
        [SerializeField]
        Statistics m_stats = new Statistics()
        {
            m_level = 1,
            m_hitPoint = 24250,
            m_attack = 3635,
            m_defense = 3635,
            m_charm = 0,
            m_sense = 0,
            m_dignity = 0,
            m_knowledge = 0,
            m_durability = 0,
            m_muscularStrength = 0,
        };
        /// <summary>1levelUP毎の固有ステータス用振り分けポイント追加量</summary>
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
            return currentLevel >= MaxLevel ? 0 : m_requiredExps[currentLevel - 1];
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

            // レベルアップする直前のレベルに合わせてステータスを上昇させる
            if (Stats.m_level < 50) // レベル50以下なら
            {
                Stats.m_hitPoint += 50;
                Stats.m_attack += 15;
                Stats.m_defense += 15;
            }
            else if (Stats.m_level >= 50 && Stats.m_level < 100) // レベル50~99なら
            {
                Stats.m_hitPoint += 25;
                Stats.m_attack += 10;
                Stats.m_defense += 10;
            }
            else if (Stats.m_level >= 100 && Stats.m_level < 150) // レベル100~149なら
            {
                Stats.m_hitPoint += 10;
                Stats.m_attack += 5;
                Stats.m_defense += 5;
            }
            else if (Stats.m_level >= 150 && Stats.m_level < 200) // レベル150~199なら
            {
                Stats.m_hitPoint += 5;
                Stats.m_attack += 1;
                Stats.m_defense += 1;
            }

            Stats.m_level++; // levelを1上げる
            m_allocationPoint += m_addStatsPoint; // レベルが上がる毎にステータスに振り分ける事が可能なポイントを一定値渡す
        }



        /// <summary>
        /// 現在のマギアのステータスを取得する
        /// 参照渡しにならない様に各値を代入して新しいインスタンスを生成して返す
        /// </summary>
        /// <returns>現在のマギアのステータス</returns>
        public Statistics GetStats()
        {
            Statistics result = new Statistics()
            {
                m_level = Stats.m_level,
                m_hitPoint = Stats.m_hitPoint,
                m_attack = Stats.m_attack,
                m_defense = Stats.m_defense,
                m_durability = Stats.m_durability,
                m_muscularStrength = Stats.m_muscularStrength,
                m_knowledge = Stats.m_knowledge,
                m_sense = Stats.m_sense,
                m_charm = Stats.m_charm,
                m_dignity = Stats.m_dignity,
            };
            return result;
        }

        /// <summary>
        /// 強化画面で編集したStatsをmagiaにセットし、固有ステータスを基礎ステータスに反映させる
        /// </summary>
        public void Update(Statistics stats = null)
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



        /// <summary>
        /// ステージ開始時,InitStateの時にその時のマギアのHP最大値で初期化する
        /// </summary>
        /// <param name="maxHP">Max hp.</param>
        public void InitMaxHP(int maxHP)
        {
            MaxHP = maxHP;
        }

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
        [Flags]
        [Serializable]
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
            AllSkill = 8191,
        }
    }
}
