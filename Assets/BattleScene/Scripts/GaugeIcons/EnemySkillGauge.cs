using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity.BattleScene
{
    public class EnemySkillGauge : MonoBehaviour
    {

        /// <summary>スキルゲージ50%時の画像</summary>
        [SerializeField] Image m_halflyGaugeIcon;
        /// <summary>スキルゲージ100%時の画像</summary>
        [SerializeField] Image m_fullyGaugeIcon;
        /// <summary>UniqueSkillGaugeのアルファカラーチャンネル(不透明度)</summary>
        [SerializeField, Range(0, 1)] float m_alphaChannel;
        /// <summary>BattleManagerの参照</summary>
        BattleManager m_battleManager;
    }
}