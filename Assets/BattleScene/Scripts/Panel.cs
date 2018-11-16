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

    public class Panel : MonoBehaviour
    {
        /// <summary>パネルが持つパネルの種類情報</summary>
        public PanelType m_panelType { get; set; }
        /// <summary>既に呼ばれたかどうか判断するフラグ</summary>
        bool m_processed = true;
        /// <summary>同オブジェクトの ChangeSprite への参照</summary>
        ChangeSprite m_changeSprite;
        /// <summary>パネルのテクスチャ</summary>
        [SerializeField] Sprite[] m_panelTextures;
        /// <summary>同オブジェクトの SpriteRenderer の参照</summary>
        SpriteRenderer m_spriteRender;

        /// <summary>
        /// フラグを初期値に戻してまた呼べる様にする
        /// </summary>
        public void ResetFlag()
        {
            m_processed = true; //フラグリセット
        }

        /// <summary>
        /// 回転してパネルの中身を見せる
        /// </summary>
        public void Working()
        {
            if (m_processed) //フラグがオフならメソッド終了
            {
                return;
            }
            StartCoroutine(Processing());
        }

        //選択されたら一回だけ演出を出してパネルの中身を表示する
        IEnumerator Processing()
        {
            Rotate(gameObject, 'y', 5f); // 回転させて3秒間立ったら止めて中身表示
            yield return new WaitForSeconds(3f); 
            ChangingSprite();
            m_processed = false; // 一回呼ばれたらtrueにする迄呼ばれない様にする
        }

        /// <summary>スプライトを変更させる : Changing sprite</summary>
        void ChangingSprite()
        {
            m_spriteRender.sprite = m_panelTextures[(int)m_panelType]; //パネルタイプのenum値をキャストで渡す
        }

        /// <summary>
        /// goをaxisを軸に1440f度time掛けて回す
        /// </summary>
        /// <param name="go">Go.</param>
        /// <param name="axis">Axis.</param>
        /// <param name="time">Time.</param>
        public void Rotate(GameObject go, char axis, float time)
        {
            iTween.RotateTo(go, iTween.Hash(axis, 1440f, "time", time));
        }
    }
}
