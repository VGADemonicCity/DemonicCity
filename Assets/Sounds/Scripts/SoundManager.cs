using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace DemonicCity
{
    public class SoundManager : MonoSingleton<SoundManager>
    {


        public enum SoundTag
        {
            BGM,
            SE,
            Voice,
            Master,
        }


        List<AudioSource> BgmSources = new List<AudioSource>();
        List<AudioSource> SESources = new List<AudioSource>();
        List<AudioSource> VoiceSources = new List<AudioSource>();

        Dictionary<SoundTag, List<AudioSource>> AudioSources
        {
            get
            {
                Dictionary<SoundTag, List<AudioSource>> tmp = new Dictionary<SoundTag, List<AudioSource>>();
                tmp.Add(SoundTag.BGM, BgmSources);
                tmp.Add(SoundTag.SE, SESources);
                tmp.Add(SoundTag.Voice, VoiceSources);
                return tmp;
            }
        }


        SoundAsset GetBgmAsset { get { return sounds[(int)SoundTag.BGM]; } }
        SoundAsset GetSEAsset { get { return sounds[(int)SoundTag.SE]; } }
        SoundAsset GetVoiceAsset { get { return sounds[(int)SoundTag.Voice]; } }

        float storyVol = -20f;

        public AudioMixer mixer;
        [SerializeField] SoundAsset[] sounds;
        [SerializeField] BGMAsset bgms;
        [SerializeField] SEAsset ses;
        [SerializeField] TutorialVoiceAsset tutorials;

        [SerializeField] string title = "Title";
        [SerializeField] string battle = "Battle";
        [SerializeField] string story = "Story";
        [SerializeField] string endCredit = "EndCredit";

        public EnemySkillSEAsset enemySkillSE;
        public SkillSEAsset magiaSkillSE;



        [SerializeField] SnapAsset snapData;

        // Use this for initialization
        void Start()
        {
            //Init();
            //LoadVol();

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnInitialize()
        {
            base.OnInitialize();
            DontDestroyOnLoad(Instance);
            Init();
        }



        void BGMCheck(Scene scene, LoadSceneMode mode = LoadSceneMode.Single)
        {

            SceneFader.SceneTitle sceneTitle;

            if (EnumCommon.TryParse(scene.name, out sceneTitle))
            {
                snapData.SceneSnaps[sceneTitle].TransitionTo(snapData.interval);
                LoadVol();
                StopWithFade(SoundTag.Voice);
                switch (sceneTitle)
                {
                    case SceneFader.SceneTitle.Battle:
                        StopWithFade(SoundTag.BGM);
                        break;
                    case SceneFader.SceneTitle.Title:
                        foreach (AudioSource item in BgmSources)
                        {
                            item.loop = true;
                        }
                        PlayWithFade(SoundAsset.BGMTag.Title);
                        break;
                    case SceneFader.SceneTitle.EndCredit:
                        foreach (AudioSource item in BgmSources)
                        {
                            item.loop = false;
                        }
                        PlayWithFade(SoundAsset.BGMTag.ThemeSong);
                        break;
                    default:
                        PlayWithFade(SoundAsset.BGMTag.Home);
                        break;
                }

            }

        }

        public void Init()
        {

            LoadVol();

            SceneManager.sceneLoaded += BGMCheck;

            if (BgmSources.Count * SESources.Count * VoiceSources.Count == 0)
            {
                foreach (Transform item in transform)
                {
                    Destroy(item.gameObject);
                }
                BgmSources.Clear();
                SESources.Clear();
                VoiceSources.Clear();
                foreach (AudioSource source in GetBgmAsset.audioSource)
                {
                    var tmp = Instantiate(source.gameObject, transform).GetComponent<AudioSource>();
                    tmp.outputAudioMixerGroup = source.outputAudioMixerGroup;

                    BgmSources.Add(tmp);
                }
                foreach (AudioSource source in GetSEAsset.audioSource)
                {
                    var tmp = Instantiate(source.gameObject, transform).GetComponent<AudioSource>();
                    tmp.outputAudioMixerGroup = source.outputAudioMixerGroup;

                    SESources.Add(tmp);
                }
                foreach (AudioSource source in GetVoiceAsset.audioSource)
                {
                    var tmp = Instantiate(source.gameObject, transform).GetComponent<AudioSource>();
                    tmp.outputAudioMixerGroup = source.outputAudioMixerGroup;

                    VoiceSources.Add(tmp);
                }
            }

            BGMCheck(SceneManager.GetActiveScene());

        }






        bool GetVol(SoundTag tag)
        {
            if (tag == SoundTag.Master)
            {
                return true;
            }

            return !AudioSources[tag][0].mute;

        }
        public void SwitchVol(SoundTag tag, bool isOn)
        {
            MuteAudioSource(tag, !isOn);
        }
        public void SwitchVol(SceneFader.SceneTitle sceneTitle, Vols vols)
        {
            snapData.SceneSnaps[sceneTitle].TransitionTo(snapData.interval);

            bool[] tmpVol = vols.vol;

            MuteAudioSource(SoundTag.BGM, !tmpVol[(int)SoundTag.BGM]);
            MuteAudioSource(SoundTag.SE, !tmpVol[(int)SoundTag.SE]);
            MuteAudioSource(SoundTag.Voice, !tmpVol[(int)SoundTag.Voice]);
        }
        public readonly string volKey = "volKey";


        Dictionary<SoundTag, string> keys = new Dictionary<SoundTag, string>()
        {
            {SoundTag.BGM,"BGMVol" },
            {SoundTag.SE,"SEVol" },
            {SoundTag.Voice,"VoiceVol" },
            {SoundTag.Master,"MasterVol" },
                    };
        public Vols CurrentVol
        {
            get
            {
                Vols vs = JsonUtility.FromJson<Vols>(PlayerPrefs.GetString(volKey));
                if (vs == null)
                {
                    return new Vols(true, true, true, true);
                }
                return vs;
            }
        }

        public void SaveVol()
        {
            string s = JsonUtility.ToJson(new Vols(GetVol(SoundTag.Master), GetVol(SoundTag.BGM), GetVol(SoundTag.SE), GetVol(SoundTag.Voice)));
            PlayerPrefs.SetString(volKey, s);
        }

        public void LoadVol()
        {
            Vols vs = JsonUtility.FromJson<Vols>(PlayerPrefs.GetString(volKey));
            if (vs == null)
            {
                vs = new Vols();
            }
            SceneFader.SceneTitle sceneTitle;
            if (EnumCommon.TryParse(SceneManager.GetActiveScene().name, out sceneTitle))
            {
                snapData.SceneSnaps[sceneTitle].TransitionTo(snapData.interval);
                SwitchVol(sceneTitle, vs);
            }
        }


        [System.Serializable]
        public class Vols
        {
            //public bool master;
            //public bool bgm;
            //public bool se;
            //public bool voice;
            public Vols(bool m, bool b, bool s, bool v)
            {
                vol[0] = b;
                vol[1] = s;
                vol[2] = v;
                vol[3] = m;
            }
            public Vols()
            {
                vol = new bool[4] { true, true, true, true };
            }
            public bool[] vol = new bool[4];
        }


        void PlaySuport(List<AudioSource> list, AudioClip clip, float fade = 0.1f)
        {
            AudioSource emptySouce = list.FirstOrDefault(x => x.isPlaying == false);

            AudioSource playingSouce = list.FirstOrDefault(x => x.isPlaying == true);
            if (playingSouce != null)
            {
                StartCoroutine(playingSouce.StopWithFadeOut(fade));
                if (emptySouce == null)
                {
                    emptySouce = list.FirstOrDefault(x => x.GetHashCode() != playingSouce.GetHashCode());
                }
            }

            StartCoroutine(emptySouce.PlayWithFadeIn(clip, fade));
        }

        /// <summary>音源再生</summary>
        /// <param name="tag">再生する音の種類</param>
        /// <param name="clip">音源</param>
        /// <param name="fade">二種類以上ある場合の音のフェード時間</param>
        public void PlayWithFade(SoundTag tag, AudioClip clip, float fade = 0.1f)
        {
            List<AudioSource> tmpList = new List<AudioSource>();
            switch (tag)
            {
                case SoundTag.BGM:
                    tmpList = BgmSources;
                    break;
                case SoundTag.SE:
                    tmpList = SESources;
                    break;
                case SoundTag.Voice:
                    tmpList = VoiceSources;
                    break;
                default:
                    break;
            }


            PlaySuport(tmpList, clip, fade);
        }
        /// <summary>音源再生</summary>
        /// <param name="tag">流すBGMの種類</param>
        /// <param name="fade">二種類以上ある場合の音のフェード時間</param>
        public void PlayWithFade(SoundAsset.BGMTag tag, float fade = 0.1f)
        {
            AudioClip clip = bgms.GetClip(tag);

            PlaySuport(BgmSources, clip, fade);

        }
        /// <summary>音源再生</summary>
        /// <param name="tag">流すSEの種類</param>
        /// <param name="fade">二種類以上ある場合の音のフェード時間</param>
        public void PlayWithFade(SoundAsset.SETag tag, float fade = 0.1f)
        {
            AudioClip clip = ses.GetClip(tag);

            PlaySuport(SESources, clip, fade);
        }
        /// <summary>音源再生</summary>
        /// <param name="tag">チュートリアルの種類</param>
        /// <param name="index">何番目か</param>
        /// <param name="fade">二種類以上ある場合の音のフェード時間</param>
        public void PlayWithFade(SoundAsset.TutorialTag tag, int index, float fade = 0.1f)
        {
            AudioClip clip = tutorials.GetClip(tag, index);

            PlaySuport(VoiceSources, clip, fade);
        }


        /// <summary>
        /// 全AudioSource再生停止
        /// </summary>
        void StopWithFade()
        {
            BgmSources.ForEach(x => StartCoroutine(x.StopWithFadeOut()));
            SESources.ForEach(x => StartCoroutine(x.StopWithFadeOut()));
            VoiceSources.ForEach(x => StartCoroutine(x.StopWithFadeOut()));
        }

        /// <summary>
        /// AudioSourceごとに再生停止
        /// </summary>
        /// <param name="tag">停止したいAudioSourceのTag</param>
        public void StopWithFade(SoundTag tag)
        {
            switch (tag)
            {
                case SoundTag.BGM:
                    BgmSources.ForEach(x => StartCoroutine(x.StopWithFadeOut()));
                    break;
                case SoundTag.SE:
                    SESources.ForEach(x => StartCoroutine(x.StopWithFadeOut()));
                    break;
                case SoundTag.Voice:
                    VoiceSources.ForEach(x => StartCoroutine(x.StopWithFadeOut()));
                    break;
                default:
                    break;
            }
        }

        public void MuteAudioSource(SoundTag tag, bool isMute)
        {
            List<AudioSource> tmpSource = new List<AudioSource>();
            switch (tag)
            {
                case SoundTag.BGM:
                    tmpSource = BgmSources;
                    break;
                case SoundTag.SE:
                    tmpSource = SESources;
                    break;
                case SoundTag.Voice:
                    tmpSource = VoiceSources;
                    break;
                default:
                    break;
            }
            foreach (AudioSource item in tmpSource)
            {
                item.mute = isMute;
            }
        }


        public void AddAudioSource(SoundTag tag, AudioSource source)
        {
            switch (tag)
            {
                case SoundTag.BGM:
                    BgmSources.Add(source);
                    break;
                case SoundTag.SE:
                    SESources.Add(source);
                    break;
                case SoundTag.Voice:
                    VoiceSources.Add(source);
                    break;
                default:
                    break;
            }
        }

    }
}