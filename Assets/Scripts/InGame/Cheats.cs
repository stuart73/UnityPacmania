using UnityEngine;
using Pacmania.InGame.Characters.Pacman;
using Pacmania.InGame.LevelStates;
using Pacmania.GameManagement;

namespace Pacmania.InGame
{
    public class Cheats : MonoBehaviour
    {
        private Level level;
        private void Awake()
        {
            level = FindObjectOfType<Level>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                level.Pacman.GetComponent<PacmanCollision>().Invincible = true;
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                level.Pacman.GetComponent<PacmanCollision>().Invincible = false;
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
                level.WinLevel();
            }
        }
    }
}
