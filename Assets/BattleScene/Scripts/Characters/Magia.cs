using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DemonicCity
{
    public class Magia : CharacterObject
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

        static Magia m_instance;
        public static Magia Instance
        {
            get
            {
                return m_instance;
            }
        }


        /// <summary>マギアの属性</summary>
        public Attribute m_attribute { get; private set; }
        /// <summary>クラスのメンバ情報をJsonファイルに書き出すクラス</summary>
        SaveData m_saveData = SaveData.Instance; // セーブデータの参照


        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            if (m_instance == null) // instanceがnullなら自分自身をインスタンスにする
            {
                m_instance = this;
                DontDestroyOnLoad(this);
            }
            else // 既に存在していた場合は自分自身を破壊する
            {
                Destroy(m_instance);
            }

            //m_myStatus.m_attack = 200;


            SceneManager.sceneLoaded += (scene, loadSceneMode) => // sceneロード時,データを再読み込みする
            {
                //m_saveData.Reload(); // reload
                Debug.Log("loaded");

                m_saveData.Save(); // save
                //m_instance = m_saveData
            };
        }
        private void Start()
        {
            Debug.Log(m_instance.m_myStatus.m_attack);
            Debug.Log(m_saveData.m_magia.m_myStatus.m_attack);
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
