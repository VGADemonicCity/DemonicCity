using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace DemonicCity
{
    /// <summary>
    /// Magia.
    /// </summary>
    public class Magia : MonoSingleton<Magia>
    {
        /// <summary>クラスのメンバ情報をJsonファイルに書き出すクラス</summary>
        SaveData m_saveData = SaveData.Instance; // セーブデータの参照
        /// <summary>ステータスクラス</summary>
        public SaveData.Statistics m_stats;

        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            m_stats = m_saveData.m_statistics; // セーブしておいたステータスを代入
            SceneManager.sceneLoaded += (scene, loadSceneMode) => // sceneロード時,データを再読み込みする
            {
                m_saveData.Save(m_stats); // save
            };
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
