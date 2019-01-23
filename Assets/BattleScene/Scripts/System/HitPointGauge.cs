using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Hit point gauge.
    /// </summary>
    public class HitPointGauge : MonoBehaviour
    {
        /// <summary>HP描画用のImageComponent</summary>
        Image m_image;
        /// <summary>HPの最大値</summary>
        float m_maxHP;
        float targetRatio;
        float changeRatio;
        /// <summary>HPの変化にかけるフレーム数</summary>
        [SerializeField] float m_drawSpeed = 1f;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Awake()
        {
            m_image = GetComponent<Image>();
        }

        /// <summary>引数の値を最大HPと現在のHPに割り当てる</summary>
        /// <param name="max">HPの最大値</param>
        public void Initialize(int max)
        {
            m_maxHP = max;
            Sync(m_maxHP);
        }


        /// <summary>
        /// Sync the specified currentHP.
        /// </summary>
        /// <param name="currentHP">Current hp.</param>
        public void Sync(float currentHP)
        {
            targetRatio = currentHP / m_maxHP;
            changeRatio = m_image.fillAmount - targetRatio;
            StartCoroutine(Drawing());
        }

        public void FullGauge()
        {
            StartCoroutine(FullGameDrawing());
        }

        /// <summary>
        /// Drawing the specified changeRatio.
        /// </summary>
        IEnumerator Drawing()
        {

            var changePerFrame = changeRatio * Time.deltaTime * m_drawSpeed;
            var remainingProcess = changeRatio; // 変動させる比率がプラス(ダメージ)ならそのまま、マイナス(回復)なら引数がマイナスなので符合を逆にして代入
            var changeValue = changePerFrame;
            while (remainingProcess > 0)
            {
                if (m_image.fillAmount - targetRatio < 0)
                {
                    changeValue = -changeValue;
                }
                m_image.fillAmount -= changePerFrame;
                remainingProcess -= changeValue;
                yield return null; // 1frame待つ
            }
        }

       public  IEnumerator FullGameDrawing()
        {
            while (m_image.fillAmount < 1f)
            {
                m_image.fillAmount += Time.deltaTime * m_drawSpeed;
                yield return null;
            }
        }
    }
}