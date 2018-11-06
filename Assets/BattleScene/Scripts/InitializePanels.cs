using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


namespace DemonicCity.BattleScene
{
    /// <summary>
    /// 各確率：
    /// Enemy:2/27,0.0740740740740
    /// CityTriple:3/27,0.1111111111111
    /// CityDouble:6/27,0.2222222222222
    /// City:16/27,0.5925925925925
    /// </summary>
    public class InitializePanels : MonoBehaviour
    {
        private PanelType m_panelType;
        private PanelType[] m_panelTypes;
        public PanelType[] GetRandomPanels()
        {
            m_panelTypes = new PanelType[27];

            //Debug.Log("elements.ToArray().Length is : " + m_panelTypes.Length);
            for (int i = 0; i < m_panelTypes.ToArray().Length; i++)
            {
                if(i <= 1)
                {
                    m_panelTypes[i] = PanelType.Enemy;
                }
                else if(i <= 4)
                {
                    m_panelTypes[i] = PanelType.CityTriple;
                }
                else if(i <= 10)
                {
                    m_panelTypes[i] = PanelType.CityDouble;
                }
                else
                {
                    m_panelTypes[i] = PanelType.City;
                }
            }
            PanelType[] array = m_panelTypes;
            PanelType[] array2 = array.OrderBy(i => Guid.NewGuid()).ToArray();//ラムダ式。配列を変換して？シャッフルして配列に戻し代入
            m_panelTypes = array2;
            return m_panelTypes;
        }
    }
}