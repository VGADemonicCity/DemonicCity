using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace DemonicCity
{
    /// <summary>
    /// Enemies factory.
    /// </summary>
    public class EnemiesFactory : SingletonBase<EnemiesFactory>
    {
        /// <summary>バトルに登場させる敵オブジェクトのリスト</summary>
        List<GameObject> enemies = new List<GameObject>();

        /// <summary>
        /// Chapterクラスに設定されているIdを元にそのチャプターで登場する敵オブジェクトをIdの順番通りにリストにして返す
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="chapter">Chapter.</param>
        public List<GameObject> Create(Chapter chapter)
        {
            chapter.enemiesIds.ForEach((enemyId) =>
            {
                enemies.Add(Resources.Load<GameObject>(enemyId.ToString()));
            });
            return enemies;
        }

        /// <summary>敵キャラクターのID</summary>
        public enum EnemiesId
        {
            /// <summary>ナフラ</summary>
            Nafla,
            /// <summary>フィニクス</summary>
            Phoenix,
            /// <summary>アーモン</summary>
            Amon,
            /// <summary>アシュメダイ</summary>
            Ashmedy,
            /// <summary>フォーラス</summary>
            Faulus,
            /// <summary>バアル</summary>
            Barl,
            /// <summary>イクスマギナ</summary>
            Ixmagina,
            /// <summary>一本角魔族</summary>
            SingleHorn,
            /// <summary>二本角魔族</summary>
            DoubleHorns,
            /// <summary>反面魔族長</summary>
            HalfMask,
            /// <summary>セトゥラス</summary>
            Setulus,
            /// <summary>超越フェニックス</summary>
            IxPheonix,
        }
    }
}
