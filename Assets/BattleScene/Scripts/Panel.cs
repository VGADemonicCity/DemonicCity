using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene   
{
    /// <summary>Panelの種類を生成時に保持する為の列挙子</summary>
    public enum PanelType
    {
        City,
        CityDouble,
        CityTriple,
        Enemy
    }

    /// <summary>
    /// Panel.
    /// </summary>
    public class Panel : MonoBehaviour
    {
        /// <summary>パネルが持つパネルの種類情報</summary>
        public PanelType m_panelType { get; set; }

        /// <summary>パネルのテクスチャ</summary>
        [SerializeField] Sprite[] m_panelTextures;
        /// <summary>時点から何度回転するか</summary>
        [SerializeField] float m_rotationDegrees = 1440f;

        /// <summary>既に呼ばれたかどうか判断するフラグ</summary>
        bool m_alreadyProcessed = false;
        /// <summary>同オブジェクトの SpriteRenderer の参照</summary>
        SpriteRenderer m_spriteRender;

        private void Start()
        {
            m_spriteRender = GetComponent<SpriteRenderer>();
        }


        /// <summary>
        /// フラグを初期値に戻してまた呼べる様にする
        /// </summary>
        public void ResetFlag()
        {
            m_alreadyProcessed = false; //フラグリセット
        }

        /// <summary>
        /// 回転してパネルの中身を見せる
        /// </summary>
        public void Open()
        {
            if (m_alreadyProcessed) //フラグがオフならメソッド終了
            {
                return;
            }
            StartCoroutine(Processing());
        }

        //選択されたら一回だけ演出を出してパネルの中身を表示する
        IEnumerator  Processing()
        {
            Rotate(gameObject, 'y', 5f); // 回転させて3秒間立ったら止めて中身表示
            yield return new WaitForSeconds(3f); 
            ChangingTexture(); // PanelTypeに合わせてtextureを変える
            m_alreadyProcessed = true; // 一回呼ばれたらtrueにする迄呼ばれない様にする
        }

        /// <summary>スプライトを変更させる : Changing sprite</summary>
        void ChangingTexture()
        {
            m_spriteRender.sprite = m_panelTextures[(int)m_panelType]; //パネルタイプのenum値をキャストで渡す
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
    }
}
