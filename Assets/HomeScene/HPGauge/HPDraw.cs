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
        float currentHP = 0;
        float drawHP = 0;

        float changeValue = 0;

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
        public void Initialize(int max)
        {
            //image = GetComponent<Image>();
            maxHP = max;
            currentHP = maxHP;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="damage"></param>
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
