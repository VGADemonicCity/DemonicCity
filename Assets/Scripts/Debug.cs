using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DemonicCity
{
#if RELEASE
    public class Debug
    {
        public static void Log(object message)
        {

        }
        public static void LogError(object message)
        {

        }
        public static void LogError(object message, Object context)
        {

        }
        public static void LogWarning(object message)
        {

        }
    }
#endif
}