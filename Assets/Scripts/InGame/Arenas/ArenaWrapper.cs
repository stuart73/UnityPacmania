using UnityEngine;
using Pacmania.InGame.Characters.Pacman;

namespace Pacmania.InGame.Arenas
{
    public class ArenaWrapper : MonoBehaviour
    {
        [SerializeField] [Range(0, 1024)] private int width = 0;
        private float halfWidth;
        private Transform pacmanScreenPosition;

        private void Awake()
        {
            pacmanScreenPosition = FindObjectOfType<PacmanController>().transform;
            halfWidth = ((float)width) / 2 / Arena.spritePixelPerUnit;
        }

        public Vector3 GetClosestWrappedPosition(Vector3 position)
        {
            Vector3 pacmanPosition = pacmanScreenPosition.position;

            if (position.x + halfWidth < pacmanPosition.x)
            {
                position.x += (width / Arena.spritePixelPerUnit);
            }
            else if (position.x - halfWidth > pacmanPosition.x)
            {
                position.x -= (width / Arena.spritePixelPerUnit);
            }

            return position;
        }
    }
}
