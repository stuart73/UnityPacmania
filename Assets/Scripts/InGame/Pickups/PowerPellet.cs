using Pacmania.GameManagement;
using Pacmania.Audio;

namespace Pacmania.InGame.Pickups
{
    public class PowerPellet : Pickup
    {
        public override void OnPickedUp()
        {
            base.OnPickedUp();
            level.AudioManager.Play(SoundType.EatPowerPellet);
            level.GhostManager.FrightenAll();
        }        
    }
}
