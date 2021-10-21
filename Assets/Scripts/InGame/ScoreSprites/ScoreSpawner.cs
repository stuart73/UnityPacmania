using System;
using System.Collections.Generic;
using UnityEngine;
using Pacmania.InGame.Characters.Ghost;
using Pacmania.InGame.Pickups;

namespace Pacmania.InGame.ScoreSprites
{
    public class ScoreSpawner : MonoBehaviour
    { 
        [SerializeField] private List<ScoreRising> scoresPrefabs = default;

        public int ScoreMultipler { get; set; } = 0;
        public bool RedPelletEaten { get; set; } = false;

        public event Action Spawned = delegate { };

        private static readonly int[] GhostScoreFromEaten = { 0, 200, 400, 800, 1600, 3200, 7650 };
        private static readonly int[] GhostScoreFromEatenRedPellet = { 0, 400, 1600, 7650 };

        private void Awake()
        {
            FindObjectOfType<FrightenSiren>().SirenStoped += ScoreSpawner_SirenStoped;
        }

        private void ScoreSpawner_SirenStoped()
        {
            ScoreMultipler = 0;
        }

        public void SpawnScoreFromGhost(GhostController ghost)
        {
            ScoreMultipler++;
            int[] scoreTableToUse = RedPelletEaten == false ? GhostScoreFromEaten: GhostScoreFromEatenRedPellet;

            if (ScoreMultipler >= scoreTableToUse.Length)
            {
                ScoreMultipler = scoreTableToUse.Length - 1;
            }

            Color scoreColour = new Color();
            ScoreColour colourComponent = ghost.GetComponent<ScoreColour>();
            if (colourComponent != null)
            {
                scoreColour = colourComponent.ScoreColor;
            }

            Spawn(scoreTableToUse[ScoreMultipler], ghost.gameObject, scoreColour, true);
        }

        public void SpawnScoreFromFruit(Fruit fruit)
        {
            Spawn(fruit.Score, fruit.gameObject, fruit.ScoreColor, false);
        }

        private void Spawn(int amount, GameObject parent, Color colour, bool attach)
        {     
            GameObject risingScore = null;

            if (scoresPrefabs != null)
            {
                foreach (ScoreRising scoreRising in scoresPrefabs)
                {
                    if (scoreRising != null && scoreRising.Score == amount)
                    {
                        if (attach == false)
                        {
                            risingScore = Instantiate(scoreRising.gameObject, parent.transform.position, Quaternion.identity);
                            risingScore.GetComponent<Animator>().applyRootMotion = true;
                        }
                        else
                        {
                            risingScore = Instantiate(scoreRising.gameObject);
                            risingScore.transform.parent = parent.transform;
                        }                     
                    }
                }
            }

            if (risingScore != null)
            {
                SpriteRenderer spriterender = risingScore.GetComponent<SpriteRenderer>();
                spriterender.color = colour;
            }

            Spawned();
        }
    }
}