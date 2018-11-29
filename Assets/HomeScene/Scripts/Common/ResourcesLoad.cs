using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity
{
    public class ResourcesLoad
    {
        /// <summary>
        /// リソースを読み込みます
        /// </summary>
        /// <typeparam name="T">リソースの型</typeparam>
        /// <param name="path">リソースのファイルパス</param>
        /// <returns>読み込んだリソース</returns>
        public static T Load<T>(string path) where T : Object
        {
            return Resources.Load(path, typeof(T)) as T;
        }
    }
}