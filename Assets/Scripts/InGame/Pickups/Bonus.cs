using System;
using UnityEngine;
using Pacmania.InGame.Arenas;
using Pacmania.GameManagement;

namespace Pacmania.InGame.Pickups
{
    public abstract class Bonus : Pickup
    {
        private int frame = 0;
        private float secondsShownFor = 10;     

        private void Start()
        {     
            level.Arena.SetTilePickUp(level.Arena.BonusTile, this);
        }

        private void FixedUpdate()
        {
            frame++;
            if (frame >= secondsShownFor * Game.FramesPerSecond)
            {
                level.Arena.SetTilePickUp(level.Arena.BonusTile, null);
                Destroy(gameObject);
            }
        }
    }
}
