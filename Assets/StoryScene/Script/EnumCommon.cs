using System;

/// <summary>
/// 列挙型の汎用関数を管理するクラス
/// </summary>
namespace DemonicCity
{
    public static partial class EnumCommon
    {
        /// <summary>
        /// 指定された文字列を列挙型に変換します
        /// </summary>
        /// <typeparam name="T">列挙型</typeparam>
        /// <param name="value">変換する文字列</param>
        /// <returns>列挙型のオブジェクト</returns>
        public static T Parse<T>(string value)
        {
            return Parse<T>(value, true);
        }

        /// <summary>
        /// 指定された文字列を列挙型に変換します
        /// </summary>
        /// <typeparam name="T">列挙型</typeparam>
        /// <param name="value">変換する文字列</param>
        /// <param name="ignoreCase">大文字と小文字を区別しない場合は true</param>
        /// <returns>列挙型のオブジェクト</returns>
        public static T Parse<T>(string value, bool ignoreCase)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// 指定された文字列を列挙型に変換して成功したかどうかを返します
        /// </summary>
        /// <typeparam name="T">列挙型</typeparam>
        /// <param name="value">変換する文字列</param>
        /// <param name="result">列挙型のオブジェクト</param>
        /// <returns>正常に変換された場合は true</returns>
        public static bool TryParse<T>(string value, out T result)
        {
            return TryParse<T>(value, true, out result);
        }

        /// <summary>
        /// 指定された文字列を列挙型に変換して成功したかどうかを返します
        /// </summary>
        /// <typeparam name="T">列挙型</typeparam>
        /// <param name="value">変換する文字列</param>
        /// <param name="ignoreCase">大文字と小文字を区別しない場合は true</param>
        /// <param name="result">列挙型のオブジェクト</param>
        /// <returns>正常に変換された場合は true</returns>
        public static bool TryParse<T>(string value, bool ignoreCase, out T result)
        {
            try
            {
                result = (T)Enum.Parse(typeof(T), value, ignoreCase);
                return true;
            }
            catch
            {
                result = default(T);
                return false;
            }
        }


        public static int GetLength<T>()
        {
            return Enum.GetValues(typeof(T)).Length;
        }
    }
}