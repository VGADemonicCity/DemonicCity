using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System;

namespace DemonicCity
{
    [Serializable]
    public abstract class SavableSingletonBase<T> : ISerializationCallbackReceiver where T : new()
    {
        /// <summary>SaveDataをJsonに変換したテキスト(Reload時になんども読み込まなくて良い様に保持)</summary>
        [SerializeField] static string m_jsonText = "";

        /// <summary>singletonの実体</summary>
        static T m_instance;
        /// <summary>singletonインスタンスを取得</summary>
        /// <value>インスタンス</value>
        public static T Instance
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

        /// <summary>
        /// 引数のオブジェクトをシリアライズして返す
        /// </summary>
        /// <returns>The serialize.</returns>
        /// <param name="obj">Object.</param>
        public static string Serialize(T obj)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, obj);
            return Convert.ToBase64String(memoryStream.GetBuffer());
        }

        /// <summary>
        /// 引数のテキストを指定されたクラスにでシリアライズして返す
        /// </summary>
        /// <returns>The deserialize.</returns>
        /// <param name="str">String.</param>
        public static T Deserialize(string str)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(str));
            return (T)binaryFormatter.Deserialize(memoryStream);
        }

        /// <summary>
        /// データを再読み込みする
        /// </summary>
        protected void ReLoad()
        {
            JsonUtility.FromJsonOverwrite(GetJason(), this);
        }

        /// <summary>
        /// データを読み込む
        /// </summary>
        protected static void Load()
        {
            m_instance = JsonUtility.FromJson<T>(GetJason());
        }

        /// <summary>
        /// 保存しているJsonを取得する
        /// </summary>
        /// <returns>Jsonファイル</returns>
        static string GetJason()
        {
            // 既にJsonを取得している場合はそれを返す
            if (!string.IsNullOrEmpty(m_jsonText))
            {
                return m_jsonText;
            }

            // Jsonを保存している場所のパスを取得
            string filePath = GetSaveFilePath();

            // Jsonが存在するか調べてから取得し変換する。存在しなければ新たなクラスを作成し、それをJsonに変換する
            if (File.Exists(filePath))
            {
                m_jsonText = File.ReadAllText(filePath);
            }
            else
            {
                m_jsonText = JsonUtility.ToJson(new T());
            }

            return m_jsonText;
        }

        /// <summary>
        /// データをJsonに変換して保存する
        /// </summary>
        public void Save()
        {
            m_jsonText = JsonUtility.ToJson(this);
            File.WriteAllText(GetSaveFilePath(), m_jsonText);
        }

        /// <summary>
        /// データを全て削除し、初期化する
        /// </summary>
        public void Delete()
        {
            m_jsonText = JsonUtility.ToJson(new T());
            ReLoad();
        }

        /// <summary>
        /// 保存する場所のパスを取得する
        /// </summary>
        /// <returns>The save file path.</returns>
        static string GetSaveFilePath()
        {
            string filePath = "SaveData";

            //確認しやすい様にエディタではAssetsと同じ階層に保存し、それ以外ではApplication.persistentDataPath以下に保存する様にする
#if UNITY_EDITOR
            filePath += ".json";
#else
            filePath = Application.persistentDataPath + "/" + filePath;
#endif
            return filePath;
        }

        /// <summary>
        /// Object->Jsonに変換される前に実行されるコールバック関数
        /// 必要に応じて任意の処理を書く
        /// </summary>
        public virtual void OnBeforeSerialize()
        {
        }

        /// <summary>
        /// Json->Objectに変換された後に実行されるコールバック関数
        /// 必要に応じて任意の処理を書く
        /// </summary>
        public virtual void OnAfterDeserialize()
        {
        }
    }
}
