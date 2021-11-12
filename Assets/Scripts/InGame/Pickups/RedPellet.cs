using Pacmania.InGame.ScoreSprites;
using Pacmania.Audio;

namespace Pacmania.InGame.Pickups
{
    public class RedPellet : Bonus
    {
        public override void OnPickedUp()
        {
            base.OnPickedUp();
            level.AudioManager.Play(SoundType.EatPowerPellet);
            level.GhostManager.FrightenAll();
            level.ScoreSpawner.RedPelletEaten = true;
        }
    }
}
