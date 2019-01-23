using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    public class ConfirmWindow : PopupSystem
    {
        protected override void Allowed()
        {
            UniqueSkillManager.Instance.Activate();
        }

        protected override void Denied()
        {
            Close();
        }
    }
}
