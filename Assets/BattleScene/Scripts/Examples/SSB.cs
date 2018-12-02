using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;


namespace DemonicCity
{
    /// <summary>
    /// 前名「SavableSingletonBase」
    /// ローカルストレージにファイルとして、シリアライズしたデータを保存できるシングルトンです（iOSの場合、該当ファイルはiCloudバックアップ対象から除外します）
    /// </summary>
    abstract public class SSB<T> where T : SSB<T>, new()
    {
        protected static T m_instance;
        bool m_isLoaded;
        protected bool m_isSaving;

        public static T Instance
        {
            get
            {
                if (null == m_instance)
                {
                    var json = File.Exists(GetSavePath()) ? File.ReadAllText(GetSavePath()) : "";
                    if (json.Length > 0)
                    {
                        LoadFromJSON(json);
                    }
                    else
                    {
                        m_instance = new T();
                        m_instance.m_isLoaded = true;
                    }
                }
                return m_instance;
            }
        }

        public void Save()
        {
            Debug.Log("saveよばれた");
            if (m_isLoaded)
            {
                Debug.Log("saveよばれた2");

                m_isSaving = true;
                var path = GetSavePath();
                File.WriteAllText(path, JsonUtility.ToJson(this));
#if UNITY_IOS
            // iOSでデータをiCloudにバックアップさせない設定
            UnityEngine.iOS.Device.SetNoBackupFlag(path);
#endif
                m_isSaving = false;
            }
        }

        public void Reset()
        {
            m_instance = null;
        }

        public void Clear()
        {
            if (File.Exists(GetSavePath()))
            {
                File.Delete(GetSavePath());
            }
            m_instance = null;
        }

        public static void LoadFromJSON(string json)
        {
            try
            {
                m_instance = new T();
                m_instance = JsonUtility.FromJson<T>(json);
                m_instance.m_isLoaded = true;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        static string GetSavePath()
        {
            Debug.Log(string.Format("{0}/{1}", Application.persistentDataPath, GetSaveKey()));
            return string.Format("{0}/{1}", Application.persistentDataPath, GetSaveKey());
        }

        static string GetSaveKey()
        {
            var provider = new SHA1CryptoServiceProvider();
            var hash = provider.ComputeHash(System.Text.Encoding.ASCII.GetBytes(typeof(T).FullName));
            return BitConverter.ToString(hash);
        }
    }
}
