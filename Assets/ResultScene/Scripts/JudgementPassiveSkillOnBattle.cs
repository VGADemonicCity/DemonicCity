using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.ResultScene
{
    public class JudgementPassiveSkillOnBattle : MonoBehaviour
    {
        Magia magia;
        Magia.PassiveSkill passiveSkill;
        private bool isMastering = false;
        private GameObject[] skillContents;

        private void Awake()
        {
            magia = Magia.Instance;
        }

        /// <summary>レベルアップ時に習得したスキルを表示する</summary>
        public void MasteringPassiveSkills()
        {
            switch (magia.Stats.Level)
            {
                case 1:
                    passiveSkill = Magia.PassiveSkill.DevilsFist;
                    skillContents[0].SetActive(true);
                    skillContents[0].GetComponent<Text>().text = Magia.PassiveSkill.DevilsFist.ToString(); 
                    break;
                case 11:
                    passiveSkill = Magia.PassiveSkill.HighConcentrationMagicalAbsorption;
                    break;
                case 23:
                    break;
                case 31:
                    break;
                case 44:
                    break;
                case 58:
                    break;
                case 70:
                    break;
                case 82:
                    break;
                case 100:
                    break;
                case 111:
                    break;
                case 136:
                    break;
                case 160:
                    break;
                case 181:
                    break;
                case 198:
                    break;
                    
            }
        }

        public Magia.PassiveSkill MasterdPassiveSkill(int level)
        {
           //バトル前の習得済みスキルとバトル後の習得済みスキルを比較する

            return passiveSkill;
        }


    }
}