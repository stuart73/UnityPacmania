using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pacmania.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private List<Sound> sounds = default;
        const float audioFadeSpeed = 0.05f;
        const float audioFadeIncrementSeconds = 0.1f;

        private void Awake()
        {
            foreach (Sound sound in sounds)
            {
                sound.AudioSource = gameObject.AddComponent<AudioSource>();
                sound.AudioSource.clip = sound.audioClip;
                sound.AudioSource.loop = sound.loop;
            }         
        }

        private Sound GetSound(SoundType type)
        {
            Sound foundSound = sounds.Find(sound => sound.type == type);
            if (foundSound == null)
            {
                Debug.Log("Unable to find sound " + type.ToString(), this);
            }
            return foundSound;
        }

        public void Play(SoundType type)
        {
            if (type == SoundType.Nothing) return;
            Sound toPlay = GetSound(type);
            if (toPlay != null)
            {
                toPlay.AudioSource.volume = 1;
                toPlay.AudioSource.Play();
            }      
        }

        public void Stop(SoundType type)
        {
            if (type == SoundType.Nothing) return;
            Sound toStop = GetSound(type);
            if (toStop != null)
            {
                toStop.AudioSource.Stop();
            }
        }

        public bool IsPlaying(SoundType type)
        {
            Sound sound = GetSound(type);
            if (sound != null)
            {
                return sound.AudioSource.isPlaying;
            }
            return false;
        }

        public void PlayAndFade(SoundType type)
        {
            Play(type);
            FadeOut(type);
        }

        public void FadeOut(SoundType type)
        {
            if (type == SoundType.Nothing) return;
            Sound toFade = GetSound(type);
            if (toFade != null)
            {
                IEnumerator fader = FadeOutCoroutine(toFade.AudioSource);
                StartCoroutine(fader);
            }
        }

        private IEnumerator FadeOutCoroutine(AudioSource audioSource)
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= audioFadeSpeed;
                yield return new WaitForSeconds(audioFadeIncrementSeconds);
            }
        }
    }
}
