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
            SingleHorn1,
            SingleHorn2,
            SingleHorn3,
            SingleHorn4,
            SingleHorn5,
            SingleHorn6,
            SingleHorn7,
            SingleHorn8,
            SingleHorn9,
            SingleHorn10,
            SingleHorn11,
            SingleHorn12,
            /// <summary>二本角魔族</summary>
            DoubleHorns1,
            DoubleHorns2,
            DoubleHorns3,
            DoubleHorns4,
            DoubleHorns5,
            DoubleHorns6,
            DoubleHorns7,
            DoubleHorns8,
            DoubleHorns9,
            DoubleHorns10,
            DoubleHorns11,
            DoubleHorns12,
            /// <summary>セトゥラス</summary>
            Setulus1,
            Setulus2,
            Setulus3,
            Setulus4,
            /// <summary>超越フェニックス</summary>
            IxPheonix,
        }
    }
}
