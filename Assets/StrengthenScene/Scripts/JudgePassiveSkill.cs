using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DemonicCity.StrengthenScene
{
    public class JudgePassiveSkill : JudgementMasteringSkill
    {

        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void JudgementMastering(Magia.PassiveSkill passiveSkill)
        {
            //if (magia.MyPassiveSkill >= passiveSkill)
            //{
            //    isMastering = true;
            //    gameObject.SetActive(true);
            //    Debug.Log(passiveSkill + "は習得済み");
            //}
            //else
            //{
            //    isMastering = false;
            //    gameObject.SetActive(false);
            //    Debug.Log(passiveSkill + "は未習得");
            //}

            if ((magia.MyPassiveSkill & passiveSkill) == passiveSkill)
            {
                gameObject.SetActive(true);
                Debug.Log(passiveSkill + "は習得済み");
            }
            else
            {
                gameObject.SetActive(false);
                Debug.Log(passiveSkill + "は未習得");
            }

        }
    }
}