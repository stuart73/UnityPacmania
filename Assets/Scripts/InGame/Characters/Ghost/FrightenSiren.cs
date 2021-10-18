using UnityEngine;
using System;
using Pacmania.InGame.Characters.Ghost.GhostStates;
using Pacmania.Audio;

namespace Pacmania.InGame.Characters.Ghost
{
    public class FrightenSiren : MonoBehaviour
    {
        private AudioManager audioManager;
        private Level level;
        public event Action SirenStoped = delegate { };

        private void Awake()
        {
            level = FindObjectOfType<Level>();
            audioManager = FindObjectOfType<AudioManager>();
        }

        private void FixedUpdate()
        {
            if (audioManager.IsPlaying(SoundType.FrigtenSiren) == true)
            {
                GhostController[] ghosts = level.GhostManager.Ghosts;
                int frightenGhostCount = 0;
                foreach (GhostController ghost in ghosts)
                {
                    if (ghost.fsm.CurrrentState.GetType() == typeof(FrightenState))
                    {
                        frightenGhostCount++;
                    }
                }
                if (frightenGhostCount == 0)
                {
                    Stop();
                }
            }
        }

        public void Play()
        {
            audioManager.Play(SoundType.FrigtenSiren);
            level.ScatterChaseTimer.Paused = true;
        }

        public void Stop()
        {
            SirenStoped.Invoke();
            audioManager.Stop(SoundType.FrigtenSiren);
            level.ScatterChaseTimer.Paused = false;
        }
    }
}
