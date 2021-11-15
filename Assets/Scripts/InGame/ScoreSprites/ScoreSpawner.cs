using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pacmania.InGame.ScoreSprites
{
    public class ScoreSpawner : MonoBehaviour
    { 
        [SerializeField] private List<ScoreRising> scoresPrefabs = default;

        public event Action Spawned = delegate { };

        public void Spawn(int amount, GameObject parent, Color colour, bool attach)
        {     
            if (scoresPrefabs != null)
            {
                foreach (ScoreRising scoreRising in scoresPrefabs)
                {
                    if (scoreRising != null && scoreRising.Score == amount)
                    {
                        SpawnScorePrefab(parent, colour, attach, scoreRising);
                        break;
                    }
                }
            }

            Spawned();
        }

        private void SpawnScorePrefab(GameObject parent, Color colour, bool attach, ScoreRising scoreRising)
        {
            GameObject risingScore;
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

            if (risingScore.TryGetComponent(out SpriteRenderer spriterender) == true)
            {
                spriterender.color = colour;
            }
        }
    }
}