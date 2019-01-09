using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace DemonicCity
{
    /// <summary>
    /// Mono singleton.
    /// </summary>
    public abstract class SavableMonoSingleton<T> : MonoBehaviour, ISerializationCallbackReceiver where T : SavableMonoSingleton<T>, new()
    {
        /// <summary>
        /// The instance.
        /// </summary>
        protected static T m_instance;
        /// <summary>SaveDataをJsonに変換したテキスト(Reload時になんども読み込まなくて良い様に保持)</summary>
        [SerializeField] static string m_jsonText = "";
        [SerializeField] protected string m_filePath = "SaveData";

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                if (m_instance != null) // m_instanceが存在するならそれを返す.
                {
                    return m_instance;
                }

                System.Type type = typeof(T); // Tのタイプ型

                T instance = FindObjectOfType(type) as T; // typeの型と一致するオブジェクトを検索してTに型変換して代入

                if (instance == null) // instanceがnullなら
                {
                    string typeName = type.ToString(); // typeの文字列

                    GameObject gameObject = new GameObject(typeName, type); // typeNameという名のゲームオブジェクトを生成してtypeという名のComponentをアタッチする
                    instance = gameObject.GetComponent<T>(); // 先程作ったオブジェクトのコンポーネントを参照

                    if (instance == null) // 生成に失敗したらエラーを返す
                    {
                        Debug.LogError("Problem during the creation of " + typeName, gameObject);
                    }
                }
                else // instanceが見つけられたらInitializeを呼ぶ
                {
                    Initialize(instance);
                }
                return m_instance;
            }
        }

        /// <summary>
        /// Initialize the specified instance.
        /// </summary>
        /// <param name="instance">Instance.</param>
        static void Initialize(T instance)
        {
            if (m_instance == null) // m_instanceがnullなら
            {
                m_instance = instance; // 代入

                m_instance.OnInitialize(); // 任意の処理を呼ぶ
            }
            else if (m_instance != instance) // 既にinstanceが他で作られていた場合
            {
                DestroyImmediate(instance.gameObject); // DestoryImmediateで完全に削除(UnityはDestroyを推奨してるみたいだけど、Destroyだと消した後も検索に引っかかってしまうらしい)
            }
        }

        /// <summary>
        /// Destroy the specified instance.
        /// </summary>
        /// <param name="instance">Instance.</param>
        static void Destroyed(T instance)
        {
            if (m_instance == instance) // 自分の意思で作ったオブジェクトを削除したなら
            {
                m_instance.OnFinalize(); // 任意の処理を呼ぶ
                m_instance = null; // nullに戻す
            }
        }

        /// <summary>
        /// instantiateした時に呼ばれる
        /// 継承先で任意の処理を書く
        /// Called when the instantiated.
        /// </summary>
        public virtual void OnInitialize()
        {
            if (File.Exists(GetSaveFilePath())) // もしセーブファイルが存在するなら
            {
                Debug.Log("存在した");
                Load();
            }
            Debug.Log("存在しなかった");
            Save();
        }

        /// <summary>
        /// destroyした時に呼ばれる
        /// 継承先で任意の処理を書く
        /// Called when the Destoryed.
        /// </summary>
        public virtual void OnFinalize() { }

        /// <summary>
        /// Awake
        /// </summary>
        void Awake()
        {
            Initialize(this as T);
        }

        /// <summary>
        /// instanceを破棄した時に呼ばれる
        /// Called when destroying the instance
        /// </summary>
        void OnDestroy()
        {
            Destroyed(this as T);
        }

        /// <summary>
        /// Called when the application ends
        /// </summary>
        void OnApplicationQuit()
        {
            Destroyed(this as T);
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
            JsonUtility.FromJsonOverwrite(GetJson(), this);
        }

        /// <summary>
        /// データを読み込む
        /// </summary>
        protected void Load()
        {
            m_instance = JsonUtility.FromJson<T>(GetJson()) as T;
        }

        /// <summary>
        /// 保存しているJsonを取得する
        /// </summary>
        /// <returns>Jsonファイル</returns>
        string GetJson()
        {
            // 既にJsonを取得している場合はそれを返す
            if (!string.IsNullOrEmpty(m_jsonText))
            {
                return m_jsonText;
            }

            // Jsonを保存している場所のパスを取得
            string filePath = GetSaveFilePath();

            // Jsonが存在するか調べてから取得し変換する。存在しなければemptyを返す
            if (File.Exists(filePath))
            {
                m_jsonText = File.ReadAllText(filePath);
            }
            else
            {
                m_jsonText = "";
                Debug.Log("SaveDataがなかったよ");
                Save(); // もしセーブデータが存在しないならここで作成する
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
            m_jsonText = "";
            ReLoad();
        }

        /// <summary>
        /// 保存する場所のパスを取得する
        /// </summary>
        /// <returns>The save file path.</returns>
        string GetSaveFilePath()
        {
            //確認しやすい様にエディタではAssetsと同じ階層に保存し、それ以外ではApplication.persistentDataPath以下に保存する様にする
            string filePath;
#if UNITY_EDITOR
            filePath = m_filePath + ".json";
#else
            filePath = Application.persistentDataPath + "/" + m_filePath;
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
