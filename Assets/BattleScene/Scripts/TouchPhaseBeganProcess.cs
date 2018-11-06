using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// TouchPhase_Beganの処理クラス
    /// もし、TouchPhase.Beganの処理が二つ以上に増えてきたら共通部分だけ残してスーパークラスに変換、継承クラスを個別で作る
    /// </summary>
    public class TouchPhaseBeganProcess : MonoBehaviour
    {
        /// <summary>パネルが一回押されたらfalseにしてもう呼ばれない様にする</summary>
        private bool m_flag = true;
        private ChangeSprite m_changeSprite;

        public void Rotation()
        {
            StartCoroutine(Processing());
        }

        private void Init()
        {
            m_changeSprite = GetComponent<ChangeSprite>();
        }

        private void Awake()
        {
            Init();
        }

        //回転させる
        private IEnumerator Processing()
        {
            if (m_flag)
            {
                ITweenRotator.Rotate(gameObject, 'y', 5f);
                yield return new WaitForSeconds(3f);
                m_changeSprite.ChangingSprite();
                m_flag = false;
            }
        }
    }
}