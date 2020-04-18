using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.Battle
{
    public class EnemiesMover : MonoBehaviour
    {
        /// <summary>移動間隔</summary>
        readonly float m_movementSpacing = 5f;
        [SerializeField] float movingTime = 3f;

        /// <summary>
        /// 背後に控えている敵を前面迄動かす
        /// </summary>
        /// <returns>遷移に掛ける時間</returns>
        public float  Moving()
        {
            iTween.MoveBy(gameObject, iTween.Hash("x", m_movementSpacing, "time", movingTime));
            return movingTime;
        }
    }
}