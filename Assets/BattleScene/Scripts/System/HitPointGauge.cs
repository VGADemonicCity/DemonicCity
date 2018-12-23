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
        int m_maxHP;
        /// <summary>HPの変化にかけるフレーム数</summary>
        [SerializeField] int m_drawSpeed = 1;

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
        }

        /// <summary>
        /// Sync the specified currentHP.
        /// </summary>
        /// <param name="currentHP">Current hp.</param>
        public void Sync(float currentHP)
        {
            var targetRatio = currentHP / m_maxHP;
            var changeRatio = m_image.fillAmount - targetRatio;
            StartCoroutine(Drawing(changeRatio));
        }

        /// <summary>
        /// Drawing the specified changeRatio.
        /// </summary>
        /// <param name="changeRatio">Change ratio.</param>
         IEnumerator Drawing(float changeRatio)
        {
            var changePerFrame = changeRatio * Time.deltaTime * m_drawSpeed;
            var reminingProcess = changeRatio >= 0 ? changeRatio : -changeRatio; // 変動させる比率がプラス(ダメージ)ならそのまま、マイナス(回復)なら引数がマイナスなので符合を逆にして代入
            while (reminingProcess > 0)
            {
                m_image.fillAmount -= changePerFrame;
                reminingProcess -= changePerFrame;
                yield return null; // 1frame待つ
            }
        }
    }
}