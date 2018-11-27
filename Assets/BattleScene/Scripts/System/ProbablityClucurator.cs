using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// 確率計算用クラス
    /// </summary>
    public static class ProbabilityCalcurator
    {
        public static bool DetectFromPercent(int percent)
        {
            return DetectFromPercent((float)percent);
        }

        /// <summary>
        /// 入力した確率で判定を行う
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static bool DetectFromPercent(float percent)
        {
            int digitNum = 0;
            if (percent.ToString().IndexOf(".") > 0)
            {
                digitNum = percent.ToString().Split('.')[1].Length;
            }

            //小数点以下をなくすように乱数の上限と判定の境界を上げる
            int rate = (int)Mathf.Pow(10, digitNum);

            //乱数の上限と真と判定するボーダー設定
            int randomValueLimit = 100 * rate;
            int border = (int)(rate * percent);

            return Random.Range(0, randomValueLimit) < border;
        }
        
        /// <summary>
        /// 入力したDictから一つを決定し、そのDictのKeyを返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetDict"></param>
        /// <returns></returns>
        public static T DetermineFromDict<T>(Dictionary<T,int> targetDict)
        {
            Dictionary<T, float> targetFloatDict = new Dictionary<T, float>();

            foreach (KeyValuePair<T,int> pair in targetDict)
            {
                targetFloatDict.Add(pair.Key, pair.Value);
            }

            return DetermineFromDict(targetFloatDict);
        }

        /// <summary>
        /// 確率とその対象をまとめたDictを入力しその中からひとつを決定、対象を返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetDict"></param>
        /// <returns></returns>
        public static T DetermineFromDict<T>(Dictionary<T, float> targetDict)
        {
            //累計確率
            float totalPer = 0;
            foreach (float per in targetDict.Values)
            {
                totalPer += per;
            }

            //0~累計確率の間で乱数を作成
            float rand = Random.Range(0, totalPer);

            //乱数から各確率を引いていき、0未満になったら終了
            foreach (KeyValuePair<T,float> pair in targetDict)
            {
                rand -= pair.Value;

                if(rand < 0)
                {
                    return pair.Key;
                }
            }

            //エラー,ここに来た時はプログラムが間違っている
            Debug.LogError("抽選ができませんでした.");
            return new List<T>(targetDict.Keys)[0];
        }
    }
}
