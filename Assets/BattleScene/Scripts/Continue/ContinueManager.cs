using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

namespace DemonicCity.BattleScene
{
    public class ContinueManager : StatesBehaviour
    {
        [SerializeField] EyeAnimator m_eyeAnimator;
        //[SerializeField] ContinueWindow m_window;
        [SerializeField] CanvasGroup m_group;
        //[SerializeField] HitPointGauge m_magiaHPGauge;
        RewardBasedVideoAd m_rewardVideo;
        string appId = "ca-app-pub-3940256099942544~3347511713";
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        bool isAd = false;
        // Use this for initialization
        void Init()
        {
            m_group.alpha = 0f;
            m_group.interactable = true;
            m_group.gameObject.SetActive(false);
            m_eyeAnimator.Init();
            m_eyeAnimator.gameObject.SetActive(false);
        }
        void RequestRewardVideo()
        {
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded video ad with the request.
            m_rewardVideo.LoadAd(request, adUnitId);
        }
        protected override void Awake()
        {
            Init();
            base.Awake();
        }
        void Start()
        {
            AdMobCallbackEnter();
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
            ShowRewardVideo();
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
            yield return new WaitWhile(() => isAd);
            //Time.timeScale = 1f;
            yield return StartCoroutine(m_eyeAnimator.Translate(1f, 0f, 0.5f, a => { m_group.alpha = a; }));
            yield return StartCoroutine(m_eyeAnimator.Open());
            Init();
            m_battleManager.SetStateMachine(BattleManager.StateMachine.State.PlayerChoice);
        }
        void AdMobCallbackEnter()
        {
            Debug.Log("Enter");
            MobileAds.Initialize(appId);
            m_rewardVideo = RewardBasedVideoAd.Instance;

            m_rewardVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
            m_rewardVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
            m_rewardVideo.OnAdOpening += HandleRewardBasedVideoOpened;
            m_rewardVideo.OnAdStarted += HandleRewardBasedVideoStarted;
            m_rewardVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
            m_rewardVideo.OnAdClosed += HandleRewardBasedVideoClosed;
            m_rewardVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

            RequestRewardVideo();
        }

        void ShowRewardVideo()
        {
            StartCoroutine(WaitShowRewardVideo());
        }

        IEnumerator WaitShowRewardVideo()
        {
            Debug.Log("CalledWaitShow");
            isAd = true;
            yield return new WaitWhile(() => !m_rewardVideo.IsLoaded());
            Debug.Log("Show");
            m_rewardVideo.Show();
        }

        #region AdMobCallbacks
        /// <summary>
        /// ロード完了時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
        }
        /// <summary>
        /// ロード失敗時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            MonoBehaviour.print(
                "HandleRewardBasedVideoFailedToLoad event received with message: "
                                 + args.Message);
        }
        /// <summary>
        /// 画面いっぱいに広告が表示されたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
        }
        /// <summary>
        /// 広告再生開始時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
        }
        /// <summary>
        /// 広告を閉じた時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
        {
            isAd = false;

            MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
        }
        /// <summary>
        /// プレイヤーへの報酬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void HandleRewardBasedVideoRewarded(object sender, Reward args)
        {
            string type = args.Type;
            double amount = args.Amount;
            MonoBehaviour.print(
                "HandleRewardBasedVideoRewarded event received for "
                            + amount.ToString() + " " + type);
            isAd = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
        }
        #endregion
    }



}
