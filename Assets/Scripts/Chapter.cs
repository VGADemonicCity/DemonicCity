using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    /// <summary>
    /// 各章の情報が格納されるクラス
    /// </summary>
    [System.Serializable]
    public class Chapter
    {
        /// <summary>その章で使うEnemiesId</summary>
        public List<EnemiesDataBase.EnemiesId> enemiesIds=new List<EnemiesDataBase.EnemiesId>();
        /// <summary>その章で使うストーリー進行度</summary>
        public Progress.StoryProgress storyProgress;
        /// <summary>その章のタイトル</summary>
        public string chapterTitle;
        /// <summary>その章に戦闘があるか</summary>
        public bool isBattle;


        public string enemyA;
        public string enemyB;
        public string enemyC;
        public string progress;



        //void SetEnemy(ref EnemiesDataBase.EnemiesId[] enemyIds, EnemiesDataBase.EnemiesId enemy1, EnemiesDataBase.EnemiesId enemy2, EnemiesDataBase.EnemiesId enemy3)
        //{
        //    enemyIds[0] = enemy1;
        //    enemyIds[1] = enemy2;
        //    enemyIds[2] = enemy3;
        //}

        /// <summary>ストーリーの進行度と敵のIDを3Wave分登録する</summary>
        public Chapter(Progress.StoryProgress progress, EnemiesDataBase.EnemiesId enemy1, EnemiesDataBase.EnemiesId enemy2, EnemiesDataBase.EnemiesId enemy3)
        {
            storyProgress = progress;
            enemiesIds.Add(enemy1);
            enemiesIds.Add(enemy2);
            enemiesIds.Add(enemy3);
            //enemiesIds[0] = enemy1;
            //enemiesIds[1] = enemy2;
            //enemiesIds[2] = enemy3;
        }
        /// <summary>ストーリーの進行度とタイトル、敵のIDを3Wave分登録する</summary>
        public Chapter(string title, Progress.StoryProgress progress, EnemiesDataBase.EnemiesId enemy1, EnemiesDataBase.EnemiesId enemy2, EnemiesDataBase.EnemiesId enemy3)
        {
            chapterTitle = title;
            storyProgress = progress;
            enemiesIds.Add(enemy1);
            enemiesIds.Add(enemy2);
            enemiesIds.Add(enemy3);
            //enemiesIds[0] = enemy1;
            //enemiesIds[1] = enemy2;
            //enemiesIds[2] = enemy3;
        }
        /// <summary>コンストラクタ</summary>
        public Chapter(Chapter chapter)
        {
            chapterTitle = chapter.chapterTitle;
            Progress.StoryProgress tmpProgress;
            EnemiesDataBase.EnemiesId tmpEnemy;
            if (EnumCommon.TryParse(chapter.progress, out tmpProgress))
            {
                storyProgress = tmpProgress;
            }
            if (EnumCommon.TryParse(chapter.enemyA, out tmpEnemy))
            {
                //enemiesIds[0] = tmpEnemy;
                enemiesIds.Add(tmpEnemy);
            }
            if (EnumCommon.TryParse(chapter.enemyB, out tmpEnemy))
            {
                enemiesIds.Add(tmpEnemy);
                //enemiesIds[1] = tmpEnemy;
            }
            if (EnumCommon.TryParse(chapter.enemyC, out tmpEnemy))
            {
                enemiesIds.Add(tmpEnemy);
                //enemiesIds[2] = tmpEnemy;
            }
            isBattle = EnumCommon.TryParse(chapter.enemyA, out tmpEnemy)
                | EnumCommon.TryParse(chapter.enemyB, out tmpEnemy)
                | EnumCommon.TryParse(chapter.enemyC, out tmpEnemy);
        }
    }
}