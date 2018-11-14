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

        /// <summary>
        /// フラグを初期値に戻してまた呼べる様にする
        /// </summary>
        public void InitPanel()
        {
            m_processed = true;
        }

        /// <summary>
        /// 回転してパネルの中身を見せる
        /// </summary>
        public void Working()
        {
            if (m_processed) 
            {
                return;
            }
            StartCoroutine(Processing());
        }

        //選択されたら一回だけ演出を出してパネルの中身を表示する
        IEnumerator Processing()
        {
            ITweenRotator.Rotate(gameObject, 'y', 5f);
            yield return new WaitForSeconds(3f);
            m_changeSprite.ChangingSprite();
            m_processed = false; // 一回呼ばれたらtrueにする迄呼ばれない様にする
        }
    }
}
