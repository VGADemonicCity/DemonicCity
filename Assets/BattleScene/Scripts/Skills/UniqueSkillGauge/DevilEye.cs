using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.Battle
{
    /// <summary>
    /// 固有スキル : 魔眼
    /// </summary>
    public class DevilEye : MonoBehaviour, IUniqueSkillActivatable
    {
        /// <summary>panelを開けるのに掛ける時間</summary>
        [SerializeField] float m_processingTime = 3f;
        /// <summary>封印された敵パネル</summary>
        [SerializeField] Sprite sealdEnemyPanel;
        /// <summary>UniqueSkillのアニメーター</summary>
        [SerializeField] Animator uniqueSkillAnimator;

        /// <summary>PanelManagerの参照</summary>
        PanelManager m_panelManager;
        /// <summary>PanelFrameManagerの参照</summary>
        PanelFrameManager m_panelFrameManager;


        private void Awake()
        {
            m_panelManager = PanelManager.Instance; // 参照取得
            m_panelFrameManager = PanelFrameManager.Instance; // 参照取得
        }

        /// <summary>
        /// 魔眼発動
        /// </summary>
        public void Activate()
        {
            StartCoroutine(PlayAnimation());
        }

        IEnumerator PlayAnimation()
        {
            // play animation
            uniqueSkillAnimator.CrossFadeInFixedTime("DevilEye", 0);
            // アニメーション再生中は処理遅延
            var clipInfos = uniqueSkillAnimator.GetCurrentAnimatorClipInfo(0);
            while (clipInfos.Length == 0)
            {
                yield return null;
                clipInfos = uniqueSkillAnimator.GetCurrentAnimatorClipInfo(0);
            }
            yield return new WaitForSeconds(clipInfos[0].clip.length);

            var enemyPanel = m_panelManager.PanelsInTheScene.Find((panel) => panel.MyPanelType == PanelType.Enemy && !panel.IsOpened); // パネル枠の中から敵パネルを取得
            m_panelFrameManager.StartCoroutine(m_panelFrameManager.MovingFrame(enemyPanel.MyFramePosition)); // 敵パネルの位置情報の場所にパネルフレームを移動させる
            yield return new WaitForSeconds(m_panelFrameManager.AnimationTime);
            enemyPanel.Open(m_processingTime, sealdEnemyPanel);// スキル発動時専用の敵パネルのスプライトを渡してそれに変える
            enemyPanel.IsOpened = true;
            yield return new WaitForSeconds(m_processingTime);
            UniqueSkillManager.Instance.OnActivateSkill();
        }
    }
}
