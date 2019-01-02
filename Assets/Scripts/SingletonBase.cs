using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    public class SingletonBase<T> where T : SingletonBase<T>, new()
    {
        // singleton pattern
        protected static T m_instance;
        public static T Instance
        {
            get
            {
                if (m_instance != null)
                {
                    return m_instance;
                }
                var temp = new object();
                lock(temp)
                {
                    m_instance = new T();
                }
                if(m_instance == null)
                {
                    throw new System.NullReferenceException();
                }
                return m_instance;
            }
        }
    }
}
