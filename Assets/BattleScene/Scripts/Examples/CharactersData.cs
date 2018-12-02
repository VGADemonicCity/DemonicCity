using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DemonicCity.CharacterSystemm
{
    [Serializable]
    public class CharactersData : SavableSingletonBase<CharactersData>
    {
        public enum CharacterId { chara1, chara2, chara3 }

        /// <summary>
        /// CharacterDataのリスト
        /// </summary>
        [SerializeField]
        List<CharacterData> items;
        
        /// <summary>
        /// IDに対応するキャラの最大レベルを返します
        /// </summary>
        /// <returns>The max level.</returns>
        /// <param name="id">Identifier.</param>
        public static int GetMaxLevel(string id)
        {
            return GetCharacterData(id).MaxLevel;
        }
        
        /// <summary>
        /// IDに対応するキャラの、次のレベルに上がるために必要な経験値を返します
        /// </summary>
        /// <returns>The required exp to next level of id.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="currentLevel">Current level.</param>
        public static int GetRequiredExpToNextLevelOfIds(string id, int currentLevel)
        {
            return GetCharacterData(id).GetRequiredExpToNextLevel(currentLevel);
        }
        
        /// <summary>
        /// IDに対応するキャラデータを取得します
        /// </summary>
        /// <returns>The character data.</returns>
        /// <param name="id">Identifier.</param>
        static CharacterData GetCharacterData(string id)
        {
            return m_instance.items.First(x => x.Id == id);
        }
        
        /// <summary>
        /// キャラ毎のデータクラス
        /// </summary>
        [Serializable]
        class CharacterData
        {
            [SerializeField]
            string id;
            
            [SerializeField]
            int[] requiredExps;
            
            public string Id
            {
                get { return id; }
            }
            
            /// <summary>
            /// 初期レベルを1としたときの最大レベルを返します
            /// </summary>
            /// <value>The max level.</value>
            public int MaxLevel
            {
                get { return requiredExps.Length + 1; }
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
}
