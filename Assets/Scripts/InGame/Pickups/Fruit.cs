using UnityEngine;
using Pacmania.Audio;
using Pacmania.InGame.ScoreSprites;
using Pacmania.InGame.LevelStates;

namespace Pacmania.InGame.Pickups
{
    public class Fruit : Bonus
    {
        [SerializeField] private Color scoreColor = default;

        public Color ScoreColor 
        { 
            get { return scoreColor; }
        }

        public override void OnPickedUp()
        {
            base.OnPickedUp();
            FindObjectOfType<AudioManager>().Play(SoundType.EatFruit);
            FindObjectOfType<ScoreSpawner>().SpawnScoreFromFruit(this);
        }

    }
}
