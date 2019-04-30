using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    public class City : MonoBehaviour
    {
        Image spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<Image>();
        }

        public void OnUpdateCallBack(Color nextColor)
        {
            spriteRenderer.color = nextColor;
        }
    }
}