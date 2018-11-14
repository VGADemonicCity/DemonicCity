using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    public class SkillDraw : MonoBehaviour
    {
        enum SkillStatus { None = 0, Half = 1, Full = 2 }
        public Image[] flames = new Image[3];
        int maxSkill = 10;
        int currentSkill = 0;
        double ratio;

        public void Initialize(int max)
        {
            maxSkill = max;
            currentSkill = 0;
            Reflect(currentSkill);
        }

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

        public void SkillReset()
        {
            currentSkill = 0;
            Reflect(0);
        }

        public void Skill1Up()
        {
            if (maxSkill > currentSkill)
            {
                currentSkill += 1;
                Reflect(currentSkill);
            }
        }


    }

}
