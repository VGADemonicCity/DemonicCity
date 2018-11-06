using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// iTweenを使って回転させる
    /// </summary>
    public static class ITweenRotator
    {
        public static void Rotate(GameObject go, char axis, float time)
        {
            iTween.RotateTo(go, iTween.Hash(axis, 1440f, "time", time));
        }
    }
}
