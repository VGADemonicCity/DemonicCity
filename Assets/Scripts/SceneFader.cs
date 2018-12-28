using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace DemonicCity
{
    /// <summary>
    /// Scene fader.
    /// </summary>
    public class SceneFader : MonoSingleton<SceneFader>
    {
        /// <summary></summary>
        private Canvas m_fadeCanvas;
        /// <summary></summary>
        private Image m_fadeImage;

        /// <summary></summary>
        private float m_alpha;

        /// <summary></summary>
        public bool m_isFadeIn;
        /// <summary></summary>
        public bool m_isFadeOut;

        /// <summary></summary>
        private float m_fadeTime = 1.0f;

        private int m_nextScene;

        private void Init()
        {

        }






        /// <summary>
        /// Scene.
        /// </summary>
        public enum Scene
        {
            Battle,
            Story,
            Home,
        }

        /// <summary>
        /// Scene fade event.
        /// </summary>
        public class SceneFadeEvent : UnityEvent<Scene>
        {
            public SceneFadeEvent() { }
        }
    }
}


