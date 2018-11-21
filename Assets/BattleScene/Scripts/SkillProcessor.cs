using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Skill processor.
    /// </summary>
    public class SkillProcessor : MonoBehaviour
    {
        /// <summary>BattleManagerのシングルトンインスタンスの参照</summary>
        BattleManager m_battleManager = BattleManager.Instance;
        PanelCounter m_panelCounter;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            m_battleManager.m_behaviourByState.AddListener((states) => // ステートマシンにイベント登録
            {
                if(states != BattleManager.States.PlayerAttack) // StateがPlayerAttack以外の時は処理終了
                {
                    return;
                }


            });
        }
    }
}
