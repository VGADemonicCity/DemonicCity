using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DemonicCity
{
    /// <summary>
    /// 敵キャラクターのデータベース
    /// </summary>
    [Serializable]
    public class EnemiesDataBase : SingletonBase<EnemiesDataBase>
    {
        /// <summary>敵キャラクターのID</summary>
        public enum EnemiesId
        {
            /// <summary>ナフラ</summary>
            Nafla,
            /// <summary>フィニクス</summary>
            Phoenix,
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
        [SerializeField]
        public List<Enemy> m_items;

        public EnemiesDataBase()
        {
            m_items = new List<Enemy>
            {
                new Enemy(EnemiesId.Phoenix,700,300,190), // フィニクス
                new Enemy(EnemiesId.Nafla,1000000,7000,5000), // ナフラ
            };
        }




        /// <summary>
        /// Idと照合出来た敵キャラクターを返す
        /// </summary>
        /// <returns>The enemy data.</returns>
        /// <param name="id">Identifier.</param>
        public Enemy GetEnemyData(EnemiesId id)
        {
            return m_instance.m_items.First(x => x.Id.ToString() == id.ToString());
        }

        /// <summary>
        /// Enemy character.
        /// </summary>
        [Serializable]
        public class Enemy : MonoBehaviour
        {
            /// <summary>m_statsのプロパティ</summary>
            public Statistics Stats
            {
                get { return m_stats; }
                set { m_stats = value; }
            }

            /// <summary>敵キャラID取得プロパティ</summary>
            public string Id
            {
                get { return m_id; }
                set { m_id = value; }
            }

            /// <summary>敵キャラのID</summary>
            [SerializeField] private string m_id;
            /// <summary>ステータス</summary>
            [SerializeField] Statistics m_stats = new Statistics();
            /// <summary>経験値</summary>
            private int Experience { get; set; }
            private Animator m_animator;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:DemonicCity.EnemiesDataBase.Enemy"/> class.
            /// </summary>
            /// <param name="id">Identifier.</param>
            /// <param name="hitPoint">Hit point.</param>
            /// <param name="attack">Attack.</param>
            /// <param name="defense">Defense.</param>
            public Enemy(EnemiesId id, int hitPoint, int attack, int defense)
            {
                Id = id.ToString();
                Stats.m_hitPoint = hitPoint;
                Stats.m_attack = attack;
                Stats.m_defense = defense;
            }
        }
    }
}
