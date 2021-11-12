﻿using UnityEngine;

namespace Pacmania.InGame.Characters.Ghost
{
    public class GhostManager : MonoBehaviour
    {
        // General ghost settings
        [SerializeField] private bool ghostsChangeDirectionOnChase = true;
        public bool GhostsChangeDirectionOnChase
        {
            get { return ghostsChangeDirectionOnChase; }
        }

        [SerializeField] [Range(0.0f, 20.0f)] private float timeInFrightenState = 10;
        public float TimeInFrightenState
        {
            get { return timeInFrightenState; }
        }

        // Holds a reference to all the ghost controlles on the level
        public GhostController[] Ghosts { get; private set; }

        public FrightenSiren FrightenSiren { get; private set; }

        private void Awake()
        {
            Ghosts = FindObjectsOfType<GhostController>();
            FrightenSiren = FindObjectOfType<FrightenSiren>();
        }

        public void SetGhostsVisibility(bool value)
        {
            foreach (GhostController ghost in Ghosts)
            {
                ghost.SetVisible(value);
            }
        }

        public void FrightenAll()
        {
            foreach (GhostController ghost in Ghosts)
            {
                ghost.Fright();
            }
            FrightenSiren.Play();
        }

        public void ResetGhostStates()
        {
            foreach (GhostController ghost in Ghosts)
            {
                ghost.ResetState();
            }
        }
    }
}
