using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene   
{
    /// <summary>Panelの種類を生成時に保持する為の列挙子</summary>
    public enum PanelType
    {
        City,
        CityDouble,
        CityTriple,
        Enemy
    }

    public class Panel : MonoBehaviour
    {
        /// <summary>パネルが持つパネルの種類情報</summary>
        public PanelType m_panelType { get; set; }
    }
}
