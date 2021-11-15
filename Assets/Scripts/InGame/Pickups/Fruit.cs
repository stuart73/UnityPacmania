using UnityEngine;
using Pacmania.Audio;
using Pacmania.InGame.ScoreSprites;
using Pacmania.InGame;

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
            level.AudioManager.Play(SoundType.EatFruit);
            level.ScoreSpawner.Spawn(Score, gameObject, ScoreColor, false);
        }

    }
}
