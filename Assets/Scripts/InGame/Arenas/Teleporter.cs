using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pacmania.InGame.Arenas
{
    public class Teleporter : MonoBehaviour
    {
        [SerializeField] private int xVetical = default;
        [SerializeField] private int arenaWidth = default;

        private int arenaHalfWidth;
        void Awake()
        {
            arenaHalfWidth = arenaWidth / 2;
        }
   
        public bool TestForTeleportation(Vector3 oldPosition, ref Vector3 newPosition)
        {
            if (enabled == false)
            {
                return false;
            }
            if (xVetical > arenaHalfWidth && oldPosition.x <= xVetical && newPosition.x > xVetical)
            {
                newPosition.x -= arenaWidth;
                return true;
            }

            else if (xVetical < arenaHalfWidth && oldPosition.x >= xVetical && newPosition.x < xVetical)
            {
                newPosition.x += arenaWidth;
                return true;
            }

            return false;
        }
    }
}
