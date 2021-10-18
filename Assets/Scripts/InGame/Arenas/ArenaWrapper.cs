﻿using UnityEngine;
using Pacmania.InGame.Characters.Pacman;

namespace Pacmania.InGame.Arenas
{
    public class ArenaWrapper : MonoBehaviour
    {
        [SerializeField] private int width = 0;
        private float halfWidth;
        private Transform pacmanScreenPosition;
        private float scale = 100.0f;

        private void Awake()
        {
            pacmanScreenPosition = FindObjectOfType<PacmanController>().transform;
            halfWidth = ((float)width) / 2 / scale;
        }

        public Vector3 GetClosestWrappedPosition(Vector3 position)
        {
            Vector3 pacmanPosition = pacmanScreenPosition.position;

            if (position.x + halfWidth < pacmanPosition.x)
            {
                position.x += (width / scale);
            }
            else if (position.x - halfWidth > pacmanPosition.x)
            {
                position.x -= (width / scale);
            }

            return position;
        }
    }
}
