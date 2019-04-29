using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        /// <summary>同オブジェクトの SpriteRenderer の参照</summary>
        Image myImage;
        PanelManager panelManager;

        void Awake()
        {
            myImage = GetComponent<Image>();
            panelManager = PanelManager.Instance;
        }

        /// <summary>
        /// フラグを初期値に戻してまた呼べる様にする
        /// </summary>
        public void ResetPanel()
        {
            myImage.sprite = m_panelTextures[(int)PanelType.Default]; // パネルのtextureをDefaultに戻す
            IsOpened = false; //フラグリセット
        }

        /// <summary>
        /// 回転してパネルの中身を見せる
        /// </summary>
        /// <param name="waitTime">Wait time.</param>
        public void Open(float waitTime, Sprite sprite = null)
        {
            if (IsOpened) // パネルが既に開かれているならメソッド終了
            {
                return;
            }
            StartCoroutine(Processing(waitTime, sprite));
        }

        //選択されたら一回だけ演出を出してパネルの中身を表示する
        public IEnumerator Processing(float waitTime, Sprite sprite = null)
        {
            IsOpened = true; // 一回呼ばれたらtrueにする迄呼ばれない様にする
            // オープン前のSE再生
            SoundManager.Instance.PlayWithFade(SoundAsset.SETag.BeforeOpenPanel);
            Rotate(gameObject, 'y', waitTime); // 回転させて3秒間立ったら止めて中身表示
            yield return new WaitForSeconds(waitTime);
            ChangingTexture(sprite); // PanelTypeに合わせてtextureを変える

            switch (MyPanelType)
            {
                case PanelType.City:
                    SoundManager.Instance.PlayWithFade(SoundAsset.SETag.AfterOpenedSingle);
                    break;
                case PanelType.DoubleCity:
                    SoundManager.Instance.PlayWithFade(SoundAsset.SETag.AfterOpenedDouble);
                    break;
                case PanelType.TripleCity:
                    SoundManager.Instance.PlayWithFade(SoundAsset.SETag.AfterOpenedTriple);
                    break;
                case PanelType.Enemy:
                    SoundManager.Instance.PlayWithFade(SoundAsset.SETag.AfterOpenedEnemeyPanel);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// パネルがパネルフレームに収まる座標に存在する場合のみColliderを有効にしそれ以外の時は無効にする
        /// </summary>
        /// <param name="position"></param>
        public void CheckActivatableCollider()
        {
            // 指定最小座標より大きいか
            var isGreaterThan = 
                gameObject.transform.position.x > panelManager.EnableMinimumPosition.x
                && gameObject.transform.position.y > panelManager.EnableMinimumPosition.y;
            // 指定最大座標より小さいか
            var isLessThan =
                gameObject.transform.position.x <= panelManager.EnableMaximumPosition.x
                && gameObject.transform.position.y <= panelManager.EnableMaximumPosition.y;

            // 指定範囲内に自分自身(Panel)が存在する場合有効
            if (isGreaterThan && isLessThan)
            {
            }
            else
            {
            }
        }

        /// <summary>スプライトを変更させる : Changing sprite</summary>
        public void ChangingTexture(Sprite sprite = null)
        {
            var displaySprite = m_panelTextures[(int)MyPanelType];
            if (sprite != null)
            {
                displaySprite = sprite;
            }

            myImage.sprite = displaySprite; //パネルタイプのenum値をキャストで渡す
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
