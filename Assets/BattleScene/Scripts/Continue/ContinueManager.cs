using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    public class ContinueManager : StatesBehaviour
    {
        [SerializeField] EyeAnimator m_eyeAnimator;
        //[SerializeField] ContinueWindow m_window;
        [SerializeField] CanvasGroup m_group;
        //[SerializeField] HitPointGauge m_magiaHPGauge;
        // Use this for initialization
        void Init()
        {
            m_group.alpha = 0f;
            m_group.interactable = true;
            m_group.gameObject.SetActive(false);
            m_eyeAnimator.Init();
            m_eyeAnimator.gameObject.SetActive(false);
        }
        protected override void Awake()
        {
            Init();
            base.Awake();
        }
        void Start()
        {
            m_battleManager.m_BehaviourByState.AddListener((state) =>
            {
                if (state == BattleManager.StateMachine.State.Continue)
                {
                    //Time.timeScale = 0f;
                    StartCoroutine(OpenContinueWindow());

                }
            });
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Submit()
        {
            m_group.interactable = false;
            StartCoroutine(Continue());
        }
        public void Cancel()
        {
            m_group.interactable = false;
            //Time.timeScale = 1f;
            m_battleManager.SetStateMachine(BattleManager.StateMachine.State.Lose);
            //SceneFader.Instance.FadeOut(SceneFader.SceneTitle.StorySelect);
        }
        IEnumerator OpenContinueWindow()
        {
            m_eyeAnimator.gameObject.SetActive(true);
            yield return StartCoroutine(m_eyeAnimator.Close());
            Debug.Log("End");
            m_battleManager.m_MagiaStats.HitPoint = m_battleManager.m_MagiaStats.MaxHP; // hpをmaxに戻す
            m_magiaHPGauge.Sync(m_battleManager.m_MagiaStats.HitPoint); // HPGaugeと同期
            m_group.gameObject.SetActive(true);
            StartCoroutine(m_eyeAnimator.Translate(0f, 1f, 0.5f, a => { m_group.alpha = a; }));
        }
        public IEnumerator Continue()
        {
            //Time.timeScale = 1f;
            yield return StartCoroutine(m_eyeAnimator.Translate(1f, 0f, 0.5f, a => { m_group.alpha = a; }));
            yield return StartCoroutine(m_eyeAnimator.Open());
            Init();
            m_battleManager.SetStateMachine(BattleManager.StateMachine.State.PlayerChoice);
        }
    }
}