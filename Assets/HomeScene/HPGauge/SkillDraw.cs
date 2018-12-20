using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    /// <summary>スキルのたまり具合を描画する</summary>
    public class SkillDraw : MonoBehaviour
    {
        /// <summary>スキルのたまり具合</summary>
        enum SkillStatus { None = 0, Half = 1, Full = 2 }
        /// <summary>描画用</summary>
        public Image[] flames = new Image[3];
        /// <summary>スキル発動に必要なパネル枚数</summary>
        int maxSkill = 10;
        /// <summary>現在のスキルのたまり具合</summary>
        int currentSkill = 0;
        ///<summary>比率</summary>
        double ratio;

        /// <summary>引数の値をスキルの最大値に割り当て、スキルゲージを空にする</summary>
        /// <param name="max">スキル発動に必要なパネル枚数</param>
        public void Initialize(int max)
        {
            maxSkill = max;
            currentSkill = 0;
            Reflect(currentSkill);
        }

        /// <summary>引数とmaxSkillを比較し、その比率に合わせてスキルゲージを描画する</summary>
        /// <param name="skill"></param>
        public void Reflect(float skill)
        {
            ratio = skill / maxSkill;
            if (ratio > 0)
            {
                flames[(int)SkillStatus.Half].color = new Color(1, 1, 1, (float)ratio * 2);
                flames[(int)SkillStatus.Full].color = Color.clear;
            }
            else
            {
                flames[(int)SkillStatus.None].color = Color.white;
                flames[(int)SkillStatus.Half].color = Color.clear;
                flames[(int)SkillStatus.Full].color = Color.clear;
            }
            if (ratio > 0.5)
            {
                flames[(int)SkillStatus.Full].color = new Color(1, 1, 1, (float)(ratio - 0.5) * 2);
                flames[(int)SkillStatus.None].color = Color.clear;
            }
            if (ratio >= 1)
            {
                flames[(int)SkillStatus.Full].color = Color.white;
                flames[(int)SkillStatus.Half].color = Color.clear;
            }

        }

        /// <summary>スキルゲージを空にする</summary>
        public void SkillReset()
        {
            currentSkill = 0;
            Reflect(currentSkill);
        }

        /// <summary>スキルゲージを一枚分溜める</summary>
        public void Skill1Up()
        {
            if (maxSkill > currentSkill)
            {
                currentSkill += 1;
                Reflect(currentSkill);
            }
        }

        /// <summary>引数の値分、スキルゲージを溜める</summary>
        /// <param name="num"></param>
        public void SkillCharge(int num)
        {
            for (int i = 0; i < num; i++)
            {
                if (maxSkill > currentSkill)
                {
                    currentSkill += 1;
                    Reflect(currentSkill);
                }
            }
        }
    }
}
