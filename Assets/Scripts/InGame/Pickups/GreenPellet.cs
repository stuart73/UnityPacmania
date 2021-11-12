using Pacmania.Audio;
using Pacmania.InGame.Characters;
using Pacmania.InGame.Characters.Pacman;

namespace Pacmania.InGame.Pickups
{
    public class GreenPellet : Bonus
    {
        public override void OnPickedUp()
        {
            base.OnPickedUp();
            level.AudioManager.Play(SoundType.EatPowerPellet);
            level.Pacman.GetComponent<CharacterMovement>().SpeedCoefficient = 1.25f;
        }
    }
}
