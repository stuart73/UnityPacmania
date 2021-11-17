using UnityEngine;
using System;
using Pacmania.InGame.Characters.Ghost.GhostStates;
using Pacmania.Audio;

namespace Pacmania.InGame.Characters.Ghost
{
    public class FrightenSiren : MonoBehaviour
    {
        private Level level;
        public event Action SirenStoped = delegate { };

        private void Awake()
        {
            level = FindObjectOfType<Level>();
        }

        private void FixedUpdate()
        {
            if (level.AudioManager.IsPlaying(SoundType.FrigtenSiren) == true)
            {
                GhostController[] ghosts = level.GhostManager.Ghosts;
                bool foundFrightenGhost = false;
                foreach (GhostController ghost in ghosts)
                {
                    if (ghost.FSM.CurrrentState.GetType() == typeof(FrightenState))
                    {
                        foundFrightenGhost = true;
                        break;
                    }
                }
                if (foundFrightenGhost == false)
                {
                    Stop();
                }
            }
        }

        public void Play()
        {
            level.AudioManager.Play(SoundType.FrigtenSiren);
            level.GhostManager.ScatterChaseTimer.Paused = true;
        }

        public void Stop()
        {
            SirenStoped.Invoke();
            level.AudioManager.Stop(SoundType.FrigtenSiren);
            level.GhostManager.ScatterChaseTimer.Paused = false;
        }
    }
}
