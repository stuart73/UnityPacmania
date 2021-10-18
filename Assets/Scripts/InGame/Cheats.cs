using UnityEngine;
using Pacmania.InGame.Characters.Pacman;
using Pacmania.InGame.LevelStates;
using Pacmania.GameManagement;

namespace Pacmania.InGame
{
    public class Cheats : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                FindObjectOfType<PacmanCollision>().Invincible = true;
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                FindObjectOfType<PacmanCollision>().Invincible = false;
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                Game.Instance.CurrentSession.InfiniteLives = true;
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                Game.Instance.CurrentSession.InfiniteLives = false;
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                FindObjectOfType<Level>().FSM.SetState(typeof(WinState));
            }
        }
    }
}
