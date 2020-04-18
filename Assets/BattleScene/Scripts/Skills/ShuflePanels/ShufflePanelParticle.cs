using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.Battle
{
    /// <summary>
    /// shuffleSkillのパーティクル管理クラス
    /// </summary>
    public class ShufflePanelParticle : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] ParticleSystem m_particleSystem;
        ParticleSystem.MainModule m_mainModule;
        ParticleSystem.EmissionModule m_emittionModule;
        PanelCounter m_panelCounter;
        [SerializeField] ParticleSystem m_waveParticle;
        ShufflePanels m_shufflePanels;

        [Header("Parameters")]
        [SerializeField] float xRotation = 0F;
        [SerializeField] float yRotation = 0F;
        [SerializeField] float zRotation = 0F;
        [SerializeField] float m_InvokeInterbal = 0.016f;
        [SerializeField] float m_speed = -0.1f;
        [SerializeField] float m_rateOverTime = 10;

        readonly float m_defaultSpeed = 0.1f;
        readonly float m_speedRatio = 0.073f;
        readonly float m_maxSpeed = -2.3f;
        readonly float m_defaultRateOverTime = 10;
        readonly float m_rateOverTimeRatio = 1.33f;
        readonly float m_maxRateOverTime = 50;
        bool m_flag = true;

        private void Awake()
        {
            m_panelCounter = PanelCounter.Instance;
            m_mainModule = m_particleSystem.main;
            m_emittionModule = m_particleSystem.emission;
            m_shufflePanels = transform.parent.GetComponent<ShufflePanels>();
        }

        private void Update()
        {
            Sync();
        }
        void OnEnable()
        {
            InvokeRepeating("Rotate", 0f, m_InvokeInterbal);
        }
        void OnDisable()
        {
            CancelInvoke();
        }
        void Rotate()
        {
            this.transform.localEulerAngles += new Vector3(xRotation, yRotation, zRotation);
        }


        /// <summary>
        /// 同期
        /// </summary>
        public void Sync()
        {
            if (m_panelCounter.CounterForShuffleSkill == 0)
            {
                m_particleSystem.Stop();
                m_flag = true;
            }
            else
            {
                if (m_particleSystem.isPaused || !m_particleSystem.isPlaying)
                {
                    m_particleSystem.Play(false);
                }
                m_speed = -(m_defaultSpeed + (m_speedRatio * m_panelCounter.CounterForShuffleSkill));
                m_rateOverTime = m_defaultRateOverTime + Mathf.Round(m_rateOverTimeRatio * m_panelCounter.CounterForShuffleSkill);

                if (m_speed < m_maxSpeed)
                {
                    m_speed = m_maxSpeed;
                }
                if (m_rateOverTime > m_maxRateOverTime)
                {
                    m_rateOverTime = m_maxRateOverTime;
                }

                m_mainModule.startSpeed = m_speed;
                m_emittionModule.rateOverTime = m_rateOverTime;

                if (m_panelCounter.CounterForShuffleSkill == m_shufflePanels.Conditions && m_flag)
                {
                    m_waveParticle.Play();
                    m_shufflePanels.OnCompleteConditions();
                    m_flag = false;
                }
            }
        }
    }
}
