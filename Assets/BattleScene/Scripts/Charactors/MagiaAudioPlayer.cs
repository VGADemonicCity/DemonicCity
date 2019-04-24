using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DemonicCity.BattleScene
{
    public class MagiaAudioPlayer : MonoBehaviour
    {
        [SerializeField] PlayerAttackState playerAttackState;
        [SerializeField] BattleStartVoiceMaterials battleStartVoiceMaterials;
        [SerializeField]  AudioClip AfterEliminatedVoiceInIxMagina;

        SoundManager soundManager;

        private void Awake()
        {
            soundManager = SoundManager.Instance;
        }

        /// <summary>
        /// 渡されたチャプターデータの章情報をもとに適切な音声を再生する
        /// </summary>
        /// <param name="chapter">そのバトルに使用されているチャプターデータ</param>
        /// <returns>音声ファイルの再生秒数</returns>
        public float PlayBattleBossVoice(Chapter chapter)
        {
            var material = battleStartVoiceMaterials.Materials.Find(mat => chapter.storyProgress == mat.StoryIdentifier);
            soundManager.PlayWithFade(SoundManager.SoundTag.Voice, material.VoiceClip);
            return material.VoiceClip.length;
        }

        /// <summary>
        /// バトル開始時に音声を再生する
        /// </summary>
        /// <param name="chapter">そのバトルに使用されているチャプターデータ</param>
        /// <returns>音声ファイルの再生秒数</returns>
        public float PlayBattleStartVoice()
        {
            var material = battleStartVoiceMaterials.Materials[0];
            soundManager.PlayWithFade(SoundManager.SoundTag.Voice, material.VoiceClip);
            return material.VoiceClip.length;
        }

        /// <summary>
        /// ixmagina専用
        /// </summary>
        /// <returns></returns>
        public float PlayVoiceInTheIxmagina()
        {
            soundManager.PlayWithFade(SoundManager.SoundTag.Voice, AfterEliminatedVoiceInIxMagina);
            return AfterEliminatedVoiceInIxMagina.length;
        }

        public void PlaySkillVoice()
        {
            playerAttackState.PlaySkillVoice();
        }

        public void PlaySkillSE()
        {
            playerAttackState.PlaySkillSE();
        }

        public void StopVoice()
        {
            playerAttackState.StopVoice();
        }
    }
}