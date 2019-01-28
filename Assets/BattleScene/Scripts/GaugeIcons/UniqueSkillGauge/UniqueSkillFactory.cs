using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// magiaの形態に応じてユニークスキルのインスタンスを返すクラス
    /// </summary>
    public class UniqueSkillFactory : MonoBehaviour
    {
        /// <summary>
        /// magiaの形態に応じてユニークスキルのインスタンスを返す
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public IUniqueSkillActivatable Create(Magia.Attribute attribute)
        {
            switch (attribute)
            {
                //==============今後作成したユニークスキルを追加していく=============
                case Magia.Attribute.Standard:
                    return GetComponentInChildren<DevilEye>();
                //case Magia.Attribute.MaleWarrior:
                //    break;
                //case Magia.Attribute.FemaleWarrior:
                //    break;
                //case Magia.Attribute.MaleWizard:
                //    break;
                //case Magia.Attribute.FemaleWitch:
                //    break;
                //case Magia.Attribute.FemaleTrancendental:
                //    break;
                default:
                    Debug.LogError("属性が設定されとらん");
                    return GetComponentInChildren<DevilEye>();
            }
        }
    }
}
