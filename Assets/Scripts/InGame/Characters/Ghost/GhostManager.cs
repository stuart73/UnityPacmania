using System;
using UnityEngine;


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
        public bool RedPelletEaten { get; set; } = false;
    
        // Holds a reference to all the ghost controlles on the level
        public GhostController[] Ghosts { get; private set; }

        public FrightenSiren FrightenSiren { get; private set; }

        private int scoreMultipler = 0;

        private static readonly int[] GhostScoreFromEaten = { 0, 200, 400, 800, 1600, 3200, 7650 };
        private static readonly int[] GhostScoreFromEatenRedPellet = { 0, 400, 1600, 7650 };

        private void Awake()
        {
            Ghosts = FindObjectsOfType<GhostController>();
            FrightenSiren = FindObjectOfType<FrightenSiren>();
            if (FrightenSiren != null)
            {
                FrightenSiren.SirenStoped += ScoreSpawner_SirenStoped;
            }
        }

        private void ScoreSpawner_SirenStoped()
        {
            scoreMultipler = 0;
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

            // If the ghosts are reset we also reset the red pellet score multiplier
            RedPelletEaten = false;
        }

        public int CalculateGhostEatenScore()
        {
            scoreMultipler++;
            int[] scoreTableToUse = RedPelletEaten == false ? GhostScoreFromEaten : GhostScoreFromEatenRedPellet;

            scoreMultipler = Math.Min(scoreMultipler, scoreTableToUse.Length - 1);
            return scoreTableToUse[scoreMultipler];
        }
    }
}
