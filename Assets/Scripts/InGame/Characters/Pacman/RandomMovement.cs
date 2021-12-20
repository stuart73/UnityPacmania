using System;
using UnityEngine;
using Pacmania.Utilities.Random;

namespace Pacmania.Utilities.Record
{
    public class RandomMovement : MonoBehaviour
    {
        [SerializeField] private int seed = 1;

        SeedUniformRandomNumberStream randomStream;

        public int Seed
        {
            get => seed;
            set => seed = value;
        }

        private void Start()
        {
            randomStream = new SeedUniformRandomNumberStream(seed);
        }

        public void Next(Vector3 currentDirection, out bool jump, out Vector2 direction)
        {
            jump = randomStream.Range(0, 180) == 4;

            direction = new Vector2(0, 0);
            int r = randomStream.Range(0, 10);

            if (r == 0 && Math.Abs(currentDirection.y) > 0)
            {
                direction.x = 1;
            }
            else if (r == 1 && Math.Abs(currentDirection.y) > 0)
            {
                direction.x = -1;
            }
            else if (r == 2 && Math.Abs(currentDirection.x) > 0)
            { 
                direction.y = 1;
            }
            else if (r == 3 && Math.Abs(currentDirection.x) > 0)
            {
                direction.y = -1;
            }
        }
    }
}
