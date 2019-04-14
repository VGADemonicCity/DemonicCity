using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity
{
    using EnemyId = EnemiesFactory.EnemiesId;
    [System.Serializable]
    [CreateAssetMenu(fileName = "EnemySkillSE", menuName = "Sounds/EnemySkillSE")]
    public class EnemySkillSEAsset : ScriptableObject
    {

        [System.Serializable]
        public class SkillSE
        {
            public EnemyId id;
            public AudioClip clip;
        }
        [SerializeField] List<SkillSE> enemySkillSE = new List<SkillSE>();

        public Dictionary<EnemyId, AudioClip> EnemySkillSE
        {
            get
            {
                Dictionary<EnemyId, AudioClip> tmp = new Dictionary<EnemyId, AudioClip>();
                foreach (var item in enemySkillSE)
                {
                    tmp.Add(item.id, item.clip);
                }
                return tmp;
            }
        }
        /// <summary>
        /// Clip取得
        /// </summary>
        /// <param name="id">ナンバリングのある敵は1の音を使う</param>
        /// <returns></returns>
        public AudioClip GetClip(EnemyId id)
        {
            switch (id)
            {
                case EnemyId.SingleHorn1:
                case EnemyId.SingleHorn2:
                case EnemyId.SingleHorn3:
                case EnemyId.SingleHorn4:
                case EnemyId.SingleHorn5:
                case EnemyId.SingleHorn6:
                case EnemyId.SingleHorn7:
                case EnemyId.SingleHorn8:
                case EnemyId.SingleHorn9:
                case EnemyId.SingleHorn10:
                case EnemyId.SingleHorn11:
                case EnemyId.SingleHorn12:
                    id = EnemyId.SingleHorn1;
                    break;
                case EnemyId.DoubleHorns1:
                case EnemyId.DoubleHorns2:
                case EnemyId.DoubleHorns3:
                case EnemyId.DoubleHorns4:
                case EnemyId.DoubleHorns5:
                case EnemyId.DoubleHorns6:
                case EnemyId.DoubleHorns7:
                case EnemyId.DoubleHorns8:
                case EnemyId.DoubleHorns9:
                case EnemyId.DoubleHorns10:
                case EnemyId.DoubleHorns11:
                case EnemyId.DoubleHorns12:
                    id = EnemyId.DoubleHorns1;
                    break;
                case EnemyId.Setulus1:
                case EnemyId.Setulus2:
                case EnemyId.Setulus3:
                case EnemyId.Setulus4:
                    id = EnemyId.Setulus1;
                    break;
                default:
                    break;
            }
            return enemySkillSE.FirstOrDefault(x => x.id == id).clip;
        }

    }
}
