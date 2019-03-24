using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


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
        List<AudioSource> MenuSESources = new List<AudioSource>();
        List<AudioSource> VoiceSources = new List<AudioSource>();


        SoundAsset GetBgmAsset { get { return sounds[(int)SoundTag.BGM]; } }
        SoundAsset GetMenuSeAsset { get { return sounds[(int)SoundTag.SE]; } }
        SoundAsset GetVoiceAsset { get { return sounds[(int)SoundTag.Voice]; } }



        public AudioMixer mixer;
        [SerializeField] SoundAsset[] sounds;

        SceneFader fader;


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


        public void Init()
        {
            fader = SceneFader.Instance;
            LoadVol();


            if (BgmSources.Count * MenuSESources.Count * VoiceSources.Count == 0)
            {
                foreach (Transform item in transform)
                {
                    Destroy(item.gameObject);
                }
                BgmSources.Clear();
                MenuSESources.Clear();
                VoiceSources.Clear();
                foreach (AudioSource source in GetBgmAsset.audioSource)
                {
                    BgmSources.Add(Instantiate(source.gameObject, transform).GetComponent<AudioSource>());
                }
                foreach (AudioSource source in GetMenuSeAsset.audioSource)
                {
                    MenuSESources.Add(Instantiate(source.gameObject, transform).GetComponent<AudioSource>());
                }
                foreach (AudioSource source in GetVoiceAsset.audioSource)
                {
                    VoiceSources.Add(Instantiate(source.gameObject, transform).GetComponent<AudioSource>());
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
                    tmpList = MenuSESources;
                    break;
                case SoundTag.Voice:
                    tmpList = VoiceSources;
                    break;
                default:
                    break;
            }

            AudioSource emptySouce = tmpList.FirstOrDefault(x => x.isPlaying == false);

            AudioSource playingSouce = tmpList.FirstOrDefault(x => x.isPlaying == true);
            if (playingSouce != null)
            {
                StartCoroutine(playingSouce.StopWithFadeOut(fade));
            }

            StartCoroutine(emptySouce.PlayWithFadeIn(clip, fade));
        }





    }
}