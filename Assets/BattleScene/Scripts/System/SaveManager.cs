using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{ 
    [Serializable]
    public class SaveManager : SavableMonoSingleton<SaveManager>
    {
        [SerializeField] Magia m_magia;


        void Awake()
        {
            m_magia = Magia.Instance;
        }

        void Start()
        {
            m_magia.Stats.m_level++;
            Save();
        }
    }
}