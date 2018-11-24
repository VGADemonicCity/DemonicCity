//  SaveData.cs
//  http://kan-kikuchi.hatenablog.com/entry/Json_SaveData
//
//  Created by kan.kikuchi on 2016.11.21.
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace DemonicCity
{

    /// <summary>
    /// クラスを丸ごとJsonで保存するデータクラス
    /// </summary>
    public class SaveData : ISerializationCallbackReceiver
    {

        //シングルトンを実装するための実体、初アクセス時にLoadする。
        static SaveData m_instance;
        public static SaveData Instance
        {
            get
            {
                if (m_instance == null)
                {
                    Load();
                }
                return m_instance;
            }
        }

        //SaveDataをJsonに変換したテキスト(リロード時に何度も読み込まなくていいように保持)
        [SerializeField]
        static string _jsonText = "";

        //=================================================================================
        //保存されるデータ(public or SerializeFieldを付ける)
        //=================================================================================

        /// <summary>マギアのステータス : Magia's statistics.</summary>
        [Serializable]
        public class Statistics
        {
            /// <summary>属性</summary>
            //[Serializable]
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
            public enum PassiveSkill
            {
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
                AmaterasuFlameWall = 2048
            }

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
            /// <summary>属性フラグ</summary>
            public Attribute m_attribute;
            /// <summary>パッシブスキルフラグ</summary>
            public PassiveSkill m_passiveSkill;
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
            public float m_hitPoint;
            /// <summary>攻撃力</summary>
            public float m_attack;
            /// <summary>防御力</summary>
            public float m_defense;
            /// <summary>レベルアップ時に得れるステータスポイント</summary>
            public float m_statusPoint;
        }
        public Statistics m_statistics;


        //=================================================================================
        //シリアライズ,デシリアライズ時のコールバック
        //=================================================================================

        /// <summary>
        /// SaveData→Jsonに変換される前に実行される。
        /// </summary>
        public void OnBeforeSerialize()
        {
            //Dictionaryはそのままで保存されないので、シリアライズしてテキストで保存。
            //m_dictJon = Serialize(m_statistics);
        }

        /// <summary>
        /// Json→SaveDataに変換された後に実行される。
        /// </summary>
        public void OnAfterDeserialize()
        {
            ////保存されているテキストがあれば、Dictionaryにデシリアライズする。
            //if (!string.IsNullOrEmpty(m_dictJon))
            //{
            //    //m_statistics = Deserialize<Dictionary<string, int>>(m_dictJon);
            //}
        }

        // ---------------------------------------------
        // 以下二つのメソッドはdictionaryを保存する場合のみ使用
        // ---------------------------------------------

        //引数のオブジェクトをシリアライズして返す
        private static string Serialize<T>(T obj)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, obj);
            return Convert.ToBase64String(memoryStream.GetBuffer());
        }

        //引数のテキストを指定されたクラスにデシリアライズして返す
        private static T Deserialize<T>(string str)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(str));
            return (T)binaryFormatter.Deserialize(memoryStream);
        }

        //=================================================================================
        //取得
        //=================================================================================

        /// <summary>
        /// データを再読み込みする。
        /// </summary>
        public void Reload()
        {
            JsonUtility.FromJsonOverwrite(GetJson(), this);
        }

        //データを読み込む。
        static void Load()
        {
            m_instance = JsonUtility.FromJson<SaveData>(GetJson());
        }

        //保存しているJsonを取得する
        static string GetJson()
        {
            //既にJsonを取得している場合はそれを返す。
            if (!string.IsNullOrEmpty(_jsonText))
            {
                return _jsonText;
            }

            //Jsonを保存している場所のパスを取得。
            string filePath = GetSaveFilePath();

            //Jsonが存在するか調べてから取得し変換する。存在しなければ新たなクラスを作成し、それをJsonに変換する。
            if (File.Exists(filePath))
            {
                _jsonText = File.ReadAllText(filePath);
            }
            else
            {
                _jsonText = JsonUtility.ToJson(new SaveData());
            }

            return _jsonText;
        }

        //=================================================================================
        //保存
        //=================================================================================

        /// <summary>
        /// データをJsonにして保存する。
        /// </summary>
        public void Save(Statistics stats)
        {
            m_statistics = stats;
            _jsonText = JsonUtility.ToJson(this);
            File.WriteAllText(GetSaveFilePath(), _jsonText);
        }

        //=================================================================================
        //削除
        //=================================================================================

        /// <summary>
        /// データを全て削除し、初期化する。
        /// </summary>
        public void Delete()
        {
            _jsonText = JsonUtility.ToJson(new SaveData());
            Reload();
        }

        //=================================================================================
        //保存先のパス
        //=================================================================================

        //保存する場所のパスを取得。
        static string GetSaveFilePath()
        {

            string filePath = "SaveData";

            //確認しやすいようにエディタではAssetsと同じ階層に保存し、それ以外ではApplication.persistentDataPath以下に保存するように。
#if UNITY_EDITOR
            filePath += ".json";
#else
    filePath = Application.persistentDataPath + "/" + filePath;
#endif

            return filePath;
        }

    }
}
