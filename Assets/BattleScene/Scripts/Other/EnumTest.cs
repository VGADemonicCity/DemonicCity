using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumTest : MonoBehaviour
{
    /// <summary>
    /// Test.
    /// </summary>
    [Flags]
    private enum Test
    {
        /// <summary>The prologue.</summary>
        Prologue = 1,
        /// <summary>The battle.</summary>
        Battle = 2,
        /// <summary>The epilogue.</summary>
        Epilogue = 4,
        /// <summary>All.</summary>
        All = Prologue | Battle | Epilogue,
        // もしくは
        /// <summary>The all2.</summary>
        All2 = 7,
    }

    Test m_test1 = Test.Prologue;
    Test m_test2 = Test.Battle;
    Test m_test3 = Test.Epilogue;
    Test m_test4 = Test.All;

    // Use this for initialization
    void Start()
    {
        Debug.Log("m_test1 : " + m_test1);
        Debug.Log("(int)m_test1 : " + (int)m_test1);
        Debug.Log("m_test1 : " + Convert.ToString((int)m_test1, 2));
        Debug.Log("m_test2 : " + m_test2);
        Debug.Log("(int)m_test2 : " + (int)m_test2);
        Debug.Log("m_test1 : " + Convert.ToString((int)m_test2, 2));
        Debug.Log("m_test3 : " + m_test3);
        Debug.Log("(int)m_test3 : " + (int)m_test3);
        Debug.Log("m_test1 : " + Convert.ToString((int)m_test3, 2));
        Debug.Log("m_test4 : " + m_test4);
        Debug.Log("(int)m_test4 : " + (int)m_test4);
        Debug.Log("m_test1 : " + Convert.ToString((int)m_test4, 2));
        Debug.Log(Test.All2);
        Debug.Log((int)Test.All2);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
