using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity
{
    public class ChapterManager : MonoBehaviour
    {


        void Awake()
        {
            SetChapter();
        }

        /// <summary>進行度と敵のIDを登録するList</summary>
        List<Chapter> chapters = new List<Chapter>();
        //{
        //    new Chapter(Progress.StoryProgress.Phoenix,EnemiesDataBase.EnemiesId.Phoenix,EnemiesDataBase.EnemiesId.Phoenix,EnemiesDataBase.EnemiesId.Phoenix),
        //    new Chapter(Progress.StoryProgress.Nafla,EnemiesDataBase.EnemiesId.Nafla,EnemiesDataBase.EnemiesId.Nafla,EnemiesDataBase.EnemiesId.Nafla),
        //}



        /// <summary>
        /// ストーリーの進行度に対応した敵のIDを取得する
        /// </summary>
        /// <param name="progress">敵のIDを知りたいストーリーの進行度</param>
        /// <returns>敵のIDのList</returns>
        public List<EnemiesDataBase.EnemiesId> GetEnemies(Progress.StoryProgress progress)
        {
            Chapter chapter = chapters.Find(item => item.storyProgress == progress);
            return chapter.enemiesIds;
        }
        /// <summary>
        /// ストーリーの進行度に対応した章のタイトルを取得する
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public string GetTitle(Progress.StoryProgress progress)
        {
            Chapter chapter = chapters.Find(item => item.storyProgress == progress);
            return chapter.chapterTitle;
        }



        /// <summary>JsonファイルのPath</summary>
        string filePath = "D:/SourceTree/DemonicCity/Assets/StoryScene/Chapters.json";
        /// <summary>
        /// JsonファイルをChapterクラスのListに変換する
        /// </summary>
        public void SetChapter()
        {
            if (null == File.ReadAllText(filePath))
            {
                return;
            }
            string chaptersJson = File.ReadAllText(filePath);
            Debug.Log(chaptersJson);
            string[] spritKey = { "><" };


            //string[] 
            List<string> tmpChapters = new List<string>();
            tmpChapters.AddRange(chaptersJson.Split(spritKey, StringSplitOptions.None));
            //Debug.Log(tmpTexts);
            //chapters = tmpChapters.Select(s => new Chapter(JsonUtility.FromJson<Chapter>(s)));
            foreach (string s in tmpChapters)
            {
                chapters.Add(new Chapter(JsonUtility.FromJson<Chapter>(s)));
            }

        }
    }
}