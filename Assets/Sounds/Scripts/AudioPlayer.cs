using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DemonicCity
{
    public static class AudioPlayer
    {

        public static bool Play(this AudioSource source, AudioClip clip = null, float vol = 1f)
        {
            if (clip == null || vol <= 0f || source == null)
            {
                return false;
            }

            source.clip = clip;
            source.volume = vol;
            source.Play();

            return true;
        }

        public static IEnumerator PlayWithFadeIn(this AudioSource source, AudioClip clip, float fadeTime = 0.1f)
        {
            if (source == null)
            {
                yield break;
            }

            //Debug.Log(source.name + "Play");
            source.Play(clip);

            while (source.volume < 1f)
            {
                float tmpVol = source.volume + (Time.deltaTime / fadeTime);

                if (tmpVol > 1f)
                {
                    source.volume = 1f;
                }
                else
                {
                    source.volume = tmpVol;
                }

                yield return null;

            }

        }

        public static IEnumerator StopWithFadeOut(this AudioSource source, float fadeTime = 0.1f)
        {
            if (source == null)
            {
                yield break;
            }
            while (source.volume > 0f)
            {
                float tmpVol = source.volume - (Time.deltaTime / fadeTime);

                if (tmpVol < 0f)
                {
                    source.volume = 0f;
                    source.Stop();
                    //Debug.LogWarning(source.name + "Stop");
                }
                else
                {
                    source.volume = tmpVol;
                }

                yield return null;

            }

        }
    }
}