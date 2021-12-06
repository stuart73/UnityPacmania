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

        public void OnInvincibleOn() => level.Pacman.GetComponent<PacmanCollision>().Invincible = true;
        public void OnInvincibleOff() => level.Pacman.GetComponent<PacmanCollision>().Invincible = false;
        public void OnInfiniteLivesOn() => Game.Instance.CurrentSession.InfiniteLives = true;
        public void OnInfiniteLivesOff() => Game.Instance.CurrentSession.InfiniteLives = false;
        public void OnEndLevelNow() => level.WinLevel();
    }
}
