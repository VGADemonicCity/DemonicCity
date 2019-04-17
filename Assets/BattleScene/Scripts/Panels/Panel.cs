using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>Panelの種類を生成時に保持する為の列挙子</summary>
    public enum PanelType
    {
        City,
        DoubleCity,
        TripleCity,
        Enemy,
        Default
    }

    /// <summary>
    /// Panel.
    /// </summary>
    public class Panel : MonoBehaviour
    {
        /// <summary>パネルが既に開かれているかどうか</summary>
        public bool IsOpened
        {
            get { return m_isOpened; }
            set { m_isOpened = value; }
        }

        /// <summary>パネルの種類</summary>
        public PanelType MyPanelType
        {
            get { return m_panelType; }
            set { m_panelType = value; }
        }

        /// <summary>パネルがいる枠の場所</summary>
        public FramePosition MyFramePosition
        {
            get { return m_framePosition; }
            set { m_framePosition = value; }
        }

        /// <summary>パネルがいる枠の場所</summary>
        [SerializeField] FramePosition m_framePosition;
        /// <summary>パネルが持つパネルの種類情報</summary>
        [SerializeField] PanelType m_panelType;
        /// <summary>既に開かれたかどうか判断するフラグ</summary>
        [SerializeField] bool m_isOpened;
        /// <summary>パネルのテクスチャ</summary>
        [SerializeField] Sprite[] m_panelTextures;
        /// <summary>時点から何度回転するか</summary>
        [SerializeField] float m_rotationDegrees = 1440f;
        /// <summary>パネルのコライダー</summary>
        public CircleCollider2D m_collider2d;
        /// <summary>同オブジェクトの SpriteRenderer の参照</summary>
        SpriteRenderer m_spriteRender;

        void Awake()
        {
            m_spriteRender = GetComponent<SpriteRenderer>(); // SpriteREndererコンポーネントの参照
            m_collider2d = GetComponent<CircleCollider2D>(); //  Collier2Dコンポーネントの参照

        }

        /// <summary>
        /// フラグを初期値に戻してまた呼べる様にする
        /// </summary>
        public void ResetPanel()
        {
            m_spriteRender.sprite = m_panelTextures[(int)PanelType.Default]; // パネルのtextureをDefaultに戻す
            IsOpened = false; //フラグリセット
        }

        /// <summary>
        /// 回転してパネルの中身を見せる
        /// </summary>
        /// <param name="waitTime">Wait time.</param>
        public void Open(float waitTime, Sprite  sprite = null)
        {
            if (IsOpened) // パネルが既に開かれているならメソッド終了
            {
                return;
            }
            StartCoroutine(Processing(waitTime,sprite));
        }

        //選択されたら一回だけ演出を出してパネルの中身を表示する
        public IEnumerator Processing(float waitTime, Sprite sprite = null)
        {
            // オープン前のSE再生
            SoundManager.Instance.PlayWithFade(SoundAsset.SETag.BeforeOpenPanel); 

            Rotate(gameObject, 'y', waitTime); // 回転させて3秒間立ったら止めて中身表示
            yield return new WaitForSeconds(waitTime);
            ChangingTexture(sprite); // PanelTypeに合わせてtextureを変える
            IsOpened = true; // 一回呼ばれたらtrueにする迄呼ばれない様にする

            // オープン後のSE再生
            SoundManager.Instance.PlayWithFade(SoundAsset.SETag.BeforeOpenPanel);
        }

        /// <summary>スプライトを変更させる : Changing sprite</summary>
        public  void ChangingTexture(Sprite sprite = null)
        {
            var displaySprite = m_panelTextures[(int)MyPanelType];
            if (sprite != null)
            {
                displaySprite = sprite;
            }

            m_spriteRender.sprite = displaySprite; //パネルタイプのenum値をキャストで渡す
        }

        /// <summary>
        /// パネルをaxis軸で1440度time秒掛けて回転させる
        /// </summary>
        /// <param name="go">Go.</param>
        /// <param name="axis">Axis.</param>
        /// <param name="time">Time.</param>
        public void Rotate(GameObject go, char axis, float time)
        {
            iTween.RotateTo(go, iTween.Hash(axis, m_rotationDegrees, "time", time));
        }

        public void Rotate(string axis, float time)
        {
            iTween.RotateTo(gameObject, iTween.Hash(axis, m_rotationDegrees, "time", time));
        }
    }
}
