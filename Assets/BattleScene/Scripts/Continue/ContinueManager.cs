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
        [SerializeField] CanvasGroup m_failedGroup;
        CanvasGroup m_currentGroup;
        //[SerializeField] HitPointGauge m_magiaHPGauge;
        RewardBasedVideoAd m_rewardVideo;
        //string appId = "ca-app-pub-7816817742729478~8064647539";
        string appId = "ca-app-pub-7816817742729478~8064647539";

        //string adUnitId = "ca-app-pub-7816817742729478/1987653339";
        string adUnitId = "ca-app-pub-7816817742729478/1987653339";
        bool isAd = false;
        bool isLoadFailed = false;
        // Use this for initialization
        void InitCanvasGroup(CanvasGroup _group)
        {
            _group.alpha = 0f;
            _group.interactable = true;
            _group.gameObject.SetActive(false);
        }
        void Init()
        {
            InitCanvasGroup(m_group);
            InitCanvasGroup(m_failedGroup);
            m_eyeAnimator.Init();
            m_eyeAnimator.gameObject.SetActive(false);
        }
        /// <summary>動画の読み込み</summary>
        void RequestRewardVideo()
        {
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded video ad with the request.
            m_rewardVideo.LoadAd(request, adUnitId);
        }
        protected override void Awake()
        {
            AdMobCallbackEnter();
            Init();
            base.Awake();
            //広告の読み込み
            RequestRewardVideo();
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
            m_currentGroup.interactable = false;
            if (isLoadFailed)
            {
                RequestRewardVideo();
            }
            else
            {
                ShowRewardVideo();
                StartCoroutine(Continue());
            }
        }
        public void Cancel()
        {
            m_currentGroup.interactable = false;
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
            m_currentGroup = m_group;
        }
        public IEnumerator Continue()
        {
            yield return new WaitWhile(() => isAd);
            //Time.timeScale = 1f;
            yield return StartCoroutine(m_eyeAnimator.Translate(1f, 0f, 0.5f, a => { m_currentGroup.alpha = a; }));
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

            //RequestRewardVideo();
        }
        /// <summary>動画再生</summary>
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

        IEnumerator TryShowRewardVideo()
        {
            yield return new WaitWhile(() => !m_rewardVideo.IsLoaded());
            if (isLoadFailed)
            {
                StartCoroutine(SwitchCanvas(m_failedGroup));
            }
            else
            {
                Submit();
            }
        }
        IEnumerator SwitchCanvas(CanvasGroup nextGroup)
        {
            yield return StartCoroutine(m_eyeAnimator.Translate(1f, 0f, 0.5f, a => { m_currentGroup.alpha = a; }));
            yield return StartCoroutine(m_eyeAnimator.Translate(0f, 1f, 0.5f, a => { nextGroup.alpha = a; }));
            m_currentGroup = nextGroup;
        }

        #region AdMobCallbacks
        /// <summary>
        /// ロード完了時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
        {
            isLoadFailed = false;
            MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
        }
        /// <summary>
        /// ロード失敗時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            isLoadFailed = true;
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
            if (isAd)
            {
                isAd = false;
                RequestRewardVideo();
            }
            else
            {

            }
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
            RequestRewardVideo();
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
