using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;



namespace DemonicCity.BattleScene
{
    public class HPDraw : MonoBehaviour
    {

        Image image = null;
        int maxHP = 1;
        /// <summary>
        /// ステータス上のHP
        /// </summary>
        float currentHP = 0;
        /// <summary>
        /// 描画されているHP
        /// </summary>
        float drawHP = 0;
        /// <summary>
        /// 1フレーム辺りに変化するHP量
        /// </summary>
        float changeValue = 0;
        /// <summary>
        /// HP判定用の遊び
        /// </summary>
        float play = 0.1f;

        void Start()
        {
            Initialize();
        }

        void Update()
        {
            drawHP = image.fillAmount * maxHP;
        }


        void Initialize()
        {
            image = GetComponent<Image>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="max"></param>
        public void Initialize(int max)
        {
            //image = GetComponent<Image>();
            maxHP = max;
            currentHP = maxHP;
        }
        /// <summary>
        /// HPゲージの値を減らす
        /// </summary>
        /// <param name="damage">ダメージ量</param>
        public void Damage(int damage)
        {
            currentHP -= damage;
            changeValue = damage / 70;
            if (changeValue == 0)
            {
                changeValue = maxHP / 1000;
            }
            if (currentHP < 0)
            {
                currentHP = 0;
                Deth();
            }

            StartCoroutine( HP());
            
        }
        /// <summary>
        /// HPゲージの値を増やす
        /// </summary>
        /// <param name="heal">回復量</param>
        public void Heal(int heal)
        {
            currentHP += heal;
            changeValue = heal / 70;
            if (changeValue == 0)
            {
                changeValue = maxHP / 1000;
            }
            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }

            StartCoroutine(HP());
            
        }
        public void Reflect(int val)
        {
            currentHP += val;
            changeValue = val / 70;

            StartCoroutine(HP());

        }

        public void Deth()
        {

        }

        IEnumerator HP()
        {

            while (currentHP + play < image.fillAmount * maxHP)
            {

                image.fillAmount -= changeValue / maxHP;

                yield return null;
            }
            while (image.fillAmount * maxHP < currentHP - play)
            {
                
                image.fillAmount += changeValue / maxHP;

                yield return null;
            }
        }

    }
}
