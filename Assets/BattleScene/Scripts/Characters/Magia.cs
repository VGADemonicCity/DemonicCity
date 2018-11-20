using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DemonicCity
{
    public class Magia : CharacterObject
    {

        void Awake()
        {
            SceneManager.sceneLoaded += (scene, sceneMode) =>
            {
                PlayerPrefs.GetInt(m_myStatus.m_level.ToString(), m_myStatus.m_level); // レベル
                PlayerPrefs.GetInt(m_myStatus.m_durability.ToString(), m_myStatus.m_durability); // 耐久力
                PlayerPrefs.GetInt(m_myStatus.m_muscularStrength.ToString(), m_myStatus.m_muscularStrength); // 筋力
                PlayerPrefs.GetInt(m_myStatus.m_knowledge.ToString(), m_myStatus.m_knowledge); // 知識
                PlayerPrefs.GetInt(m_myStatus.m_sense.ToString(), m_myStatus.m_sense); // センス
                PlayerPrefs.GetInt(m_myStatus.m_charm.ToString(), m_myStatus.m_charm); // 魅力
                PlayerPrefs.GetInt(m_myStatus.m_dignity.ToString(), m_myStatus.m_dignity); // 威厳           
            };
        }

        /// <summary>
        /// Instance生成時の初期化method
        /// </summary>
        public override void OnInitialize()
        {
            DontDestroyOnLoad(Instance); // オブジェクトをscene遷移で破壊させない様にする
        }

        /// <summary>
        /// キャラクター情報を保存する
        /// </summary>
        public void Save()
        {
            PlayerPrefs.SetInt(m_myStatus.m_level.ToString(), m_myStatus.m_level); // レベル
            PlayerPrefs.SetInt(m_myStatus.m_durability.ToString(), m_myStatus.m_durability); // 耐久力
            PlayerPrefs.SetInt(m_myStatus.m_muscularStrength.ToString(), m_myStatus.m_muscularStrength); // 筋力
            PlayerPrefs.SetInt(m_myStatus.m_knowledge.ToString(), m_myStatus.m_knowledge); // 知識
            PlayerPrefs.SetInt(m_myStatus.m_sense.ToString(), m_myStatus.m_sense); // センス
            PlayerPrefs.SetInt(m_myStatus.m_charm.ToString(), m_myStatus.m_charm); // 魅力
            PlayerPrefs.SetInt(m_myStatus.m_dignity.ToString(), m_myStatus.m_dignity); // 威厳

            PlayerPrefs.Save(); // setした情報を全て保存する
        }


        /// <summary>
        /// Levels up.
        /// </summary>
        public void LevelUp()
        {

        }

        /// <summary>
        /// Sets the statuses.
        /// </summary>
        public void SetStatuses()
        {

        }
    }
}
