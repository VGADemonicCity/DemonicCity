using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace DemonicCity
{
    [Serializable]
    public class Magia : MonoBehaviour
    {
        /// <summary>属性</summary>
        public enum Attribute
        {
            /// <summary>初期形態</summary>
            Standard,
            /// <summary>男戦士</summary>
            MaleWarrior,
            /// <summary>女戦士</summary>
            FemaleWarrior,
            /// <summary>男魔法使い</summary>
            MaleWizard,
            /// <summary>女魔法使い</summary>
            FemaleWitch
        }


        public int Aa = 1;
        /// <summary>マギアの属性</summary>
        public Attribute m_attribute;
        /// <summary>クラスのメンバ情報をJsonファイルに書き出すクラス</summary>
        SaveData m_saveData = SaveData.Instance; // セーブデータの参照
        SaveData.Statistics m_stats;


        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {

            //m_attribute = Attribute.FemaleWarrior;
            m_stats = m_saveData.m_statistics;
            m_stats.m_attack += 100;

            SceneManager.sceneLoaded += (scene, loadSceneMode) => // sceneロード時,データを再読み込みする
            {
                //m_saveData.Reload(); // reload
                Debug.Log("loaded");

                m_saveData.Save(m_stats); // save
                //m_instance = m_saveData
            };
        }
        private void Start()
        {
            Debug.Log(m_stats.m_attack);
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
