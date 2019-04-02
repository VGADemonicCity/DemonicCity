using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    public class City : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void OnUpdateCallBack(Color nextColor)
        {
            spriteRenderer.color = nextColor;
        }
    }
}