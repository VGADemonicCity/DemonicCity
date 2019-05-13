using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Scene mover.
    /// </summary>
    public class SceneMover : MonoBehaviour
    {
        SceneFader m_sceneFader;
        private void Start()
        {
            m_sceneFader = SceneFader.Instance;
        }

        public void OnClick()
        {
            m_sceneFader.FadeOut(SceneFader.SceneTitle.Home);
        }
    }
}