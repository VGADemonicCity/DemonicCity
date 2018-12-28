using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DemonicCity
{
    /// <summary>
    /// 
    /// </summary>
    public class SceneFadeManager : MonoSingleton<SceneFadeManager>
    {






        /// <summary>
        /// 
        /// </summary>
        public enum Scene
        {
            Battle,
            Story,
            Home,
        }

        /// <summary>
        /// 
        /// </summary>
        public class SceneFadeEvent : UnityEvent<Scene>
        {
            public SceneFadeEvent() { }
        }
    }
}


