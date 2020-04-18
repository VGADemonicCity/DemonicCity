using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.Battle
{
    /// <summary>
    /// Magic circle.
    /// </summary>
    public class MagicCircle : MonoBehaviour
    {
        [SerializeField] float m_angle = 100f;

        private void Update()
        {
            transform.Rotate(Vector3.forward, m_angle * Time.deltaTime);
        }
    }
}