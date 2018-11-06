using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>Spriteをランダムで切り替えるクラス</summary>
    public class ChangeSprite : MonoBehaviour
    {
        [SerializeField] private Sprite[] m_panels;
        private SpriteRenderer m_spriteRender;
        private Panel m_panel;

        /// <summary>初期化</summary>
        private void Init()
        {
            m_spriteRender = GetComponent<SpriteRenderer>();
            m_panel = GetComponent<Panel>();
        }

        private void Awake()
        {
            Init();
        }

        /// <summary>スプライトを変更させる : Changing sprite</summary>
        public void ChangingSprite()
        {
            m_spriteRender.sprite = m_panels[(int)m_panel.m_panelType]; //パネルタイプのenum値をキャストで渡す
        }

    }
}
