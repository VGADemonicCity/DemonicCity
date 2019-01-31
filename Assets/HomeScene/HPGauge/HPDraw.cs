using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;



namespace DemonicCity.BattleScene
{
    public class HPDraw : MonoBehaviour
    {
        /// <summary>HP描画用のImageComponent</summary>
        Image image = null;
        /// <summary>HPの最大値</summary>
        int maxHP = 1;
        /// <summary>ステータス上のHP</summary>
        float currentHP = 0;
        /// <summary>1フレーム辺りに変化するHP量</summary>
        float changeValue = 0;
        /// <summary>HP判定用の遊び</summary>
        float play = 0.1f;
        /// <summary>HPの変化にかけるフレーム数</summary>
        //int frame = 70;
        /// <summary>HPゲージの移動速度</summary>
        float drawSpeed = 1f;
        /// <summary>変化するHP量が少なすぎてchangeValueが0になるようなことがあった場合は最大HPの1/1000をchangeValueとする</summary>
        int divideNumber = 1000;


        void Start()
        {
            image = GetComponent<Image>();
        }

        /// <summary>引数の値を最大HPと現在のHPに割り当てる</summary>
        /// <param name="max">HPの最大値</param>
        public void Initialize(int max)
        {
            //image = GetComponent<Image>();
            maxHP = max;
            currentHP = maxHP;
        }
        /// <summary>HPゲージの値を減らす</summary>
        /// <param name="damage">ダメージ量</param>
        public void Damage(int damage)
        {
            currentHP -= damage;
            changeValue = damage * drawSpeed / Time.deltaTime;
            if (changeValue == 0)
            {
                changeValue = maxHP / divideNumber;
            }
            if (currentHP < 0)
            {
                currentHP = 0;
                Deth();
            }

            StartCoroutine(HP());

        }
        /// <summary>HPゲージの値を増やす</summary>
        /// <param name="heal">回復量</param>
        public void Heal(int heal)
        {
            currentHP += heal;
            changeValue = heal * drawSpeed / Time.deltaTime;
            if (changeValue == 0)
            {
                changeValue = maxHP / divideNumber;
            }
            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }

            StartCoroutine(HP());

        }
        /// <summary>HPゲージの値を増減させる</summary>
        /// <param name="val">+なら回復、-ならダメージ</param>
        public void Reflect(int val)
        {
            currentHP += val;
            changeValue = val * drawSpeed / Time.deltaTime;

            StartCoroutine(HP());

        }
        /// <summary>ゲージが0になるときに何か処理を入れるのならここに</summary>
        public void Deth()
        {

        }

        /// <summary>現在のHPとゲージに差異がある場合にゲージの値を現在のHPに近づける</summary>
        /// <returns></returns>
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