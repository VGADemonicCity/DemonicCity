using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    public class EnemiesMover : MonoBehaviour
    {
        /// <summary>移動間隔</summary>
        readonly float m_movementSpacing = 5f;
        [SerializeField] float movingTime = 3f;

        public void Moving()
        {
            iTween.MoveBy(gameObject, iTween.Hash("x", m_movementSpacing, "time", movingTime));
        }
    }
}