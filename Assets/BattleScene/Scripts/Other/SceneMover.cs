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
        public void MovingScene()
        {
            SceneManager.LoadScene("Battle");
        }
    }
}