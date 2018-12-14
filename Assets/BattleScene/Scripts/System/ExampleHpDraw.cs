using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;



namespace DemonicCity.BattleScene
{
    public class ExampleHpDraw : MonoBehaviour
    {
        /// <summary>HP描画用のImageComponent</summary>
        Image image = null;
        /// <summary>HPの最大値</summary>
        int maxHP = 1;
        /// <summary>ステータス上のHP</summary>
        float currentHP = 0;
        float changeRatio;
        /// <summary>HPの変化にかけるフレーム数</summary>
        [SerializeField] int drawSpeed = 1;


        void Start()
        {
            image = GetComponent<Image>();
        }

        /// <summary>引数の値を最大HPと現在のHPに割り当てる</summary>
        /// <param name="max">HPの最大値</param>
        public void Initialize(int max)
        {
            //image = GetComponent<Image>();
            currentHP = maxHP = max;
        }

        /// <summary>
        /// ダメージ.
        /// HPの変動を同期する
        /// </summary>
        /// <param name="damage">Damage.</param>
        public void Damage(int damage)
        {
            currentHP -= damage;
            changeRatio = damage * Time.deltaTime *　drawSpeed / maxHP;
            StartCoroutine(Drawing());
        }

        /// <summary>
        /// 回復.
        /// HPの変動を同期する
        /// </summary>
        /// <param name="heal">Heal.</param>
        public void Heal(int heal)
        {
            currentHP += heal;
            changeRatio = heal * Time.deltaTime * drawSpeed / maxHP;
            StartCoroutine(Drawing());
        }

        /// <summary>現在のHPとゲージに差異がある場合にゲージの値を現在のHPに近づける</summary>
        IEnumerator Drawing()
        {
            while (currentHP < image.fillAmount * maxHP && image.fillAmount > Mathf.Epsilon)
            {
                image.fillAmount -= changeRatio;
                yield return null;
            }
            while (currentHP > image.fillAmount * maxHP && image.fillAmount > Mathf.Epsilon)
            {
                image.fillAmount += changeRatio;
                yield return null;
            }
        }
    }
}