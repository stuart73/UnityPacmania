using Pacmania.GameManagement;
using Pacmania.Audio;

namespace Pacmania.InGame.Pickups
{
    public class Pellet : Pickup
    {
        public override void OnPickedUp()
        {
            base.OnPickedUp();
            level.AudioManager.Play(SoundType.EatPellet); 
        }
    }
}
