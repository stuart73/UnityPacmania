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
            Arena arena = FindObjectOfType<Arena>();
            arena.SetTilePickUp(arena.BonusTile, this);
        }

        private void FixedUpdate()
        {
            frame++;
            if (frame >= secondsShownFor * Game.FramesPerSecond)
            {
                Arena arena = FindObjectOfType<Arena>();
                arena.SetTilePickUp(arena.BonusTile, null);
                Destroy(gameObject);
            }
        }
    }
}
