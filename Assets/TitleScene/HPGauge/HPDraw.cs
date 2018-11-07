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

        void Start()
        {
            Initialize();
        }

        void Update()
        {
            drawHP = image.fillAmount * maxHP;
            if (currentHP + 0.1 < drawHP)
            {
                image.fillAmount -= changeValue / maxHP;
            }
            else if(drawHP < currentHP - 0.1)
            {
                image.fillAmount += changeValue / maxHP;
            }
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

        public void Damage(int damage)
        {
            currentHP -= damage;
            changeValue = damage / 70;
            if (changeValue == 0)
            {
                changeValue = maxHP/1000;
            }
            if (currentHP <0)
            {
                currentHP =0;
                Deth();
            }
        }
        public void Heal(int heal)
        {            
            currentHP += heal;
            changeValue = heal / 70;
            if (changeValue == 0)
            {
                changeValue = maxHP / 1000;
            }
            if (currentHP>maxHP)
            {
                currentHP = maxHP;
            }
        }
        public void Reflect(int val)
        {
            currentHP += val;
            changeValue = val / 70;

        }

        public void Deth()
        {

        }
        //public void Reflect(int max, int val)
        //{

        //    image.fillAmount = val / max;
        //}
    }
}
