using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    /// <summary>
    /// Game manager.
    /// </summary>
    public class GameManager : SavableMonoSingleton<GameManager>
    {



        /// <summary>
        /// 作ったゲームオブジェクトをシーン遷移しても壊されない様にする
        /// </summary>
        public override void OnInitialize()
        {
            base.OnInitialize();
            DontDestroyOnLoad(Instance);
        }
    }
}
