using System;
using UnityEngine;

namespace Pacmania.Audio
{
    [Serializable]
    public class Sound
    {
        public SoundType type;
        public AudioClip audioClip;
        public bool loop;
        public AudioSource AudioSource { get; set; }
    }
}
