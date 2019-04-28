using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DemonicCity
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SnapData", menuName = "Sound/Snap")]
    public class SnapAsset : ScriptableObject
    {
        [System.Serializable]
        public class SceneSnap
        {
            [Header("どのシーンで使用するか")]
            [SerializeField] SceneFader.SceneTitle sceneTitle;

            [Header("どのSnapShotを使用するか")]
            [SerializeField] AudioMixerSnapshot snap;
            public KeyValuePair<SceneFader.SceneTitle, AudioMixerSnapshot> Snap
            {
                get
                {
                    return new KeyValuePair<SceneFader.SceneTitle, AudioMixerSnapshot>(sceneTitle, snap);
                }
            }
        }

        [Header("音の切り替えにかける時間")]
        public float interval = 1f;
        [SerializeField] List<SceneSnap> snaps = new List<SceneSnap>();

        public Dictionary<SceneFader.SceneTitle, AudioMixerSnapshot> SceneSnaps
        {
            get
            {
                Dictionary<SceneFader.SceneTitle, AudioMixerSnapshot> tmp = new Dictionary<SceneFader.SceneTitle, AudioMixerSnapshot>();

                foreach (SceneSnap item in snaps)
                {
                    var pair = item.Snap;
                    tmp.Add(pair.Key, pair.Value);
                }

                return tmp;
            }
        }

    }
}