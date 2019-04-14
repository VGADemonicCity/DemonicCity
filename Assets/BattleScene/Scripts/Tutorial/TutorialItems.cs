using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonicCity.BattleScene;

namespace DemonicCity
{
    /// <summary>
    /// チュートリアルの素材クラス
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "TutorialItems", menuName = "Tutorials/TutorialItems")]
    public class TutorialItems : ScriptableObject
    {
        public List<TutorialItem> Items;

        /// <summary>
        /// チュートリアル用素材
        /// </summary>
        [System.Serializable]
        public class TutorialItem 
        {
            /// <summary>表示するテクスチャ</summary>
            public Sprite Sprite;
            /// <summary>音声再生するかどうか</summary>
            public bool useVoice;
            /// <summary>音声クリップ</summary>
            public AudioClip VoiceClip;
 
            [Header("BattleSceneでの識別用")]
            /// <summary>battle scene用識別子</summary>
            public Subject subject;

        }
    }
}
