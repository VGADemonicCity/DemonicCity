using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    public class PanelFrameArrow : MonoBehaviour
    {
        public bool IsVisible
        {
            get
            {
                return panelFrameManager.GetPanelFramePosition != invisiblePosition;
            }
        }

        /// <summary>このパネル枠の状態に応じて非表示にする</summary>
        [Header("このパネル枠の状態に応じて非表示にする")]
        [SerializeField] FramePosition invisiblePosition;
        [SerializeField] float amplitudeX;
        [SerializeField] float amplitudeY;
        [SerializeField] float speed;
        [SerializeField] float scalingTime;

        PanelFrameManager panelFrameManager;
        Image arrowImage;
        RectTransform rect;
        float timer;
        Vector3 initialPsition;
        Vector3 initialScale = new Vector3(1, 0, 0);

        private void Start()
        {
            panelFrameManager = PanelFrameManager.Instance;
            arrowImage = GetComponent<Image>();
            rect = GetComponent<RectTransform>();

            initialPsition = rect.localPosition;
            rect.localScale = initialScale;

            BattleManager.Instance.m_BehaviourByState.AddListener(state =>
            {
                if (state == BattleManager.StateMachine.State.PlayerChoice && rect.localScale == initialScale)
                {
                    iTween.ValueTo(gameObject, iTween.Hash(
                        "from", initialScale,
                        "to", Vector3.one,
                        "time", scalingTime,
                        "onupdate", "ScalingArrow",
                        "updatetarget", gameObject,
                        "ignoretimescale", false));
                }
            });
        }

        void ScalingArrow(Vector3 nextScale)
        {
            rect.localScale = nextScale;
        }

        private void Update()
        {
            if (IsVisible)
            {
                arrowImage.color = Color.white;
            }
            else
            {
                arrowImage.color = Color.clear;
            }

            timer += Time.deltaTime;
            var posX = Mathf.Sin(timer * speed) * amplitudeX;
            var posY = Mathf.Sin(timer * speed) * amplitudeY;
            rect.localPosition = initialPsition + new Vector3(posX, posY, 0);
        }
    }
}
