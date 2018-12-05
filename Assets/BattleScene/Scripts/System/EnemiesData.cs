using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace DemonicCity
{
    [Serializable]
    public class EnemiesData : SSB<EnemiesData>
        {
        /// <summary>敵キャラクターのID</summary>
        public enum EnemiesId
        {
            /// <summary>フィニクス</summary>
            Phoenix,
            /// <summary>ナフラ</summary>
            Nahura,
            /// <summary>アーモン</summary>
            Ammon,
            /// <summary>アシュメダイ</summary>
            Ashmedai,
            /// <summary>フォーラス</summary>
            Forlas,
            /// <summary>バアル</summary>
            Baal,
            /// <summary>イクスマギナ</summary>
            Exmugina
        }

        /// <summary>敵キャラクターのリスト</summary>
        [SerializeField] List<Enemy> m_items;

        /// <summary>
        ///　Idと照合出来た敵キャラクターを返す
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static Enemy GetEnemyData(string id)
        {
            return m_instance.m_items.First(x => x.Id == id);
        }

        /// <summary>
        /// Enemy character.
        /// </summary>
        [Serializable]
        class Enemy
        {
            /// <summary>敵キャラのID</summary>
            [SerializeField] string m_id;
            /// <summary>敵キャラID取得プロパティ</summary>
            public string Id { get { return m_id; } }

            /// <summary>ステータス</summary>
            [SerializeField] Statistics m_stats;
            /// <summary>m_statsのプロパティ</summary>
            public Statistics Stats
            {
                get { return m_stats; }
                set { m_stats = value; }
            }

        }
    }
}
