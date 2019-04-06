using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.StrengthenScene
{

    public class JudgementMasteringSkill : MonoBehaviour
    {
        protected Magia magia;
        public Magia.PassiveSkill m_passiveSkill;
        protected bool isMastering = false;
    
        public virtual void Awake()
        {
            magia = Magia.Instance;
        }


        public virtual void Start()
        {
            JudgementMastering(m_passiveSkill);
        }

        public virtual void JudgementMastering(Magia.PassiveSkill m_passiveSkill)
        {
            if (magia.MyPassiveSkill >= m_passiveSkill)
            {
                isMastering = true;
                gameObject.SetActive(true);
                Debug.Log(m_passiveSkill + "は習得済み");
            }
            else
            {
                isMastering = false;
                gameObject.SetActive(false);
                Debug.Log(m_passiveSkill + "は未習得");
            }
        }
    }
}
