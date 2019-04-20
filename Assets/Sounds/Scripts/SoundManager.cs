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

        public EnemySkillSEAsset enemySkillSE;
        public SkillSEAsset magiaSkillSE;


        //SceneFader fader;


        //private void Awake()
        //{

        //}

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

        void BGMCheck(Scene scene, LoadSceneMode mode)
        {
            LoadVol();

            if (CurrentVol.vol[0])
            {
                if (scene.name == story)
                {
                    mixer.SetFloat(keys[SoundTag.BGM], storyVol);
                }
                else
                {
                    mixer.SetFloat(keys[SoundTag.BGM], defVol);
                }

            }
            if (scene.name == title)
            {
                PlayWithFade(SoundAsset.BGMTag.Title);
            }
            else if (scene.name == battle)
            {
                Debug.Log("Battle");
                StopWithFade(SoundTag.BGM);
            }
            else
            {

                PlayWithFade(SoundAsset.BGMTag.Home);
            }


        }

        public void Init()
        {
            //fader = SceneFader.Instance;
            LoadVol();

            //VolCheck(SceneManager.GetActiveScene(), LoadSceneMode.Single);

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

        }






        //public float GetVol(string key)
        //{
        //    float dec;
        //    mixer.GetFloat(key, out dec);
        //    return dec;
        //    //Mathf.Pow(10f, dec / 20f);
        //}
        bool GetVol(string key)
        {
            float vol;
            if (mixer.GetFloat(key, out vol))
            {
                if (vol <= -70f)
                {
                    return false;
                }
            }
            return true;
        }
        public void SetVol(string key, float vol)
        {
            //float dec = vol;
            //float dec = 20.0f * Mathf.Log10(vol);
            //if (float.IsNegativeInfinity(dec))
            //{
            //    dec = -96f;
            //}
            mixer.SetFloat(key, vol);
        }
        float muteVol = -80f;
        float defVol = 0f;
        public void SwitchVol(string key, bool isOn)
        {
            if (isOn)
            {
                mixer.SetFloat(key, defVol);
            }
            else
            {
                mixer.SetFloat(key, muteVol);
            }
        }
        public void SwitchVol(SoundTag tag, bool isOn)
        {
            if (isOn)
            {
                mixer.SetFloat(keys[tag], defVol);
            }
            else
            {
                mixer.SetFloat(keys[tag], muteVol);
            }
        }


        public readonly string master = "MasterVol";
        public readonly string bgm = "BGMVol";
        public readonly string voice = "VoiceVol";
        public readonly string se = "SEVol";
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
            string s = JsonUtility.ToJson(new Vols(GetVol(master), GetVol(bgm), GetVol(se), GetVol(voice)));
            PlayerPrefs.SetString(volKey, s);
        }

        void LoadVol()
        {
            Vols vs = JsonUtility.FromJson<Vols>(PlayerPrefs.GetString(volKey));
            if (vs == null)
            {
                vs = new Vols();
            }
            //SetVol(master, vs.master);
            //SetVol(bgm, vs.bgm);
            //SetVol(volKey, vs.voice);
            //SetVol(se, vs.se);            
            SwitchVol(master, vs.vol[(int)SoundTag.Master]);
            SwitchVol(bgm, vs.vol[(int)SoundTag.BGM]);
            SwitchVol(se, vs.vol[(int)SoundTag.SE]);
            SwitchVol(voice, vs.vol[(int)SoundTag.Voice]);
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
                //master = m;
                //bgm = b;
                //se = s;
                //voice = v;
            }
            public Vols()
            {
                vol = new bool[4] { true, true, true, true };
            }
            public bool[] vol = new bool[4];
        }
        //public class Vols
        //{
        //    public float master;
        //    public float bgm;
        //    public float voice;
        //    public float se;
        //    public Vols(float m, float b, float v, float s)
        //    {
        //        master = m;
        //        bgm = b;
        //        voice = v;
        //        se = s;
        //    }
        //}


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
            BgmSources.ForEach(x => x.StopWithFadeOut());
            SESources.ForEach(x => x.StopWithFadeOut());
            VoiceSources.ForEach(x => x.StopWithFadeOut());
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
                    BgmSources.ForEach(x => x.StopWithFadeOut());
                    break;
                case SoundTag.SE:
                    SESources.ForEach(x => x.StopWithFadeOut());
                    break;
                case SoundTag.Voice:
                    VoiceSources.ForEach(x => x.StopWithFadeOut());
                    break;
                default:
                    break;
            }
        }


    }
}