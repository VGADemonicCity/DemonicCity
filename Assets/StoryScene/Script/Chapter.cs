using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity
{
    /// <summary>
    /// 各章の情報が格納されるクラス
    /// </summary>
    public class Chapter : MonoBehaviour
    {
        /// <summary>その章で使うEnemiesId</summary>
        public List<EnemiesDataBase.EnemiesId> enemiesIds;
        /// <summary>その章で使うストーリー進行度</summary>
        public Progress.StoryProgress storyProgress;

        //public EnemiesDataBase.EnemiesId[] GetEnemiesId(Progress.StoryProgress storyProgress)
        //{
        //    EnemiesDataBase.EnemiesId[] enemiesIds = new EnemiesDataBase.EnemiesId[3];
        //    switch (storyProgress)
        //    {
        //        case Progress.StoryProgress.Prologue:
        //            enemiesIds = null;
        //            break;
        //        case Progress.StoryProgress.Phoenix:
        //            SetEnemy(ref enemiesIds, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix);
        //            break;
        //        case Progress.StoryProgress.Nafla:
        //            SetEnemy(ref enemiesIds, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix);
        //            break;
        //        case Progress.StoryProgress.ZAKO1:
        //            SetEnemy(ref enemiesIds, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix);
        //            break;
        //        case Progress.StoryProgress.Amon:
        //            SetEnemy(ref enemiesIds, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix);
        //            break;
        //        case Progress.StoryProgress.ZAKO2:
        //            SetEnemy(ref enemiesIds, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix);
        //            break;
        //        case Progress.StoryProgress.Ashmedy:
        //            SetEnemy(ref enemiesIds, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix);
        //            break;
        //        case Progress.StoryProgress.ZAKO3:
        //            SetEnemy(ref enemiesIds, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix);
        //            break;
        //        case Progress.StoryProgress.Faulus:
        //            SetEnemy(ref enemiesIds, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix);
        //            break;
        //        case Progress.StoryProgress.ZAKO4:
        //            SetEnemy(ref enemiesIds, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix);
        //            break;
        //        case Progress.StoryProgress.Barl:
        //            SetEnemy(ref enemiesIds, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix);
        //            break;
        //        case Progress.StoryProgress.Ixmagina:
        //            SetEnemy(ref enemiesIds, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix);
        //            break;
        //        case Progress.StoryProgress.All:
        //            SetEnemy(ref enemiesIds, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix, EnemiesDataBase.EnemiesId.Phoenix);
        //            break;
        //        default:
        //            break;
        //    }
        //    return;
        //}

        /// <summary>進行度と敵のIDを登録するList</summary>
        List<Chapter> chapters = new List<Chapter>
        {
            new Chapter(Progress.StoryProgress.Phoenix,EnemiesDataBase.EnemiesId.Phoenix,EnemiesDataBase.EnemiesId.Phoenix,EnemiesDataBase.EnemiesId.Phoenix),
            new Chapter(Progress.StoryProgress.Nafla,EnemiesDataBase.EnemiesId.Nahura,EnemiesDataBase.EnemiesId.Nahura,EnemiesDataBase.EnemiesId.Nahura),
        };

        /// <summary>
        /// ストーリーの進行度に対応した敵のIDを求める
        /// </summary>
        /// <param name="progress">敵のIDを知りたいストーリーの進行度</param>
        /// <returns>敵のIDのList</returns>
        public List<EnemiesDataBase.EnemiesId> GetEnemies(Progress.StoryProgress progress)
        {
            Chapter chapter = chapters.Find(item => item.storyProgress == progress);
            return chapter.enemiesIds;
        }

        //void SetEnemy(ref EnemiesDataBase.EnemiesId[] enemyIds, EnemiesDataBase.EnemiesId enemy1, EnemiesDataBase.EnemiesId enemy2, EnemiesDataBase.EnemiesId enemy3)
        //{
        //    enemyIds[0] = enemy1;
        //    enemyIds[1] = enemy2;
        //    enemyIds[2] = enemy3;
        //}

        /// <summary>ストーリーの進行度と敵のIDを3Wave分登録する</summary>
        Chapter(Progress.StoryProgress progress, EnemiesDataBase.EnemiesId enemy1, EnemiesDataBase.EnemiesId enemy2, EnemiesDataBase.EnemiesId enemy3)
        {
            storyProgress = progress;
            enemiesIds[0] = enemy1;
            enemiesIds[1] = enemy2;
            enemiesIds[2] = enemy3;
        }
    }
}