using System;
using UnityEngine;
using Pacmania.Utilities.Sprites;
using Pacmania.Utilities.StateMachines;
using Pacmania.GameManagement;
using Pacmania.InGame.Characters.Ghost;
using Pacmania.InGame.Pickups;

namespace Pacmania.InGame.LevelStates
{
    public class DyingState : BaseState
    {
        private int frame = 0;
        private const int pacmanSpinStartFrame = 100;
        private const int pacmanSpinEndFrame = pacmanSpinStartFrame + 160;

        public override void OnStateEnter(GameObject forGameObject)
        {
            frame = 0;

            Level level = forGameObject.GetComponent<Level>();
            level.CharacterManager.PauseCharacters();
            level.Arena.GetComponent<GreyoutSprite>().GreyEnabled(true);
            level.AudioManager.FadeOut(level.Arena.Music);
        }

        public override Type Update(GameObject forGameObject)
        {
            frame++;
  
            if (frame == pacmanSpinStartFrame)
            {
                StartPacmanSpin(forGameObject);
            }
            else if (frame>= pacmanSpinEndFrame)
            {
                return DoPacmanDied(forGameObject);
            }
            return GetType();
        }

        private Type DoPacmanDied(GameObject forGameObject)
        {
            GameSession gameSession = Game.Instance.CurrentSession;

            if (gameSession.InfiniteLives == false)
            {
                gameSession.Lives--;
            }

            if (gameSession.Lives == 0)
            {
                return EndGame(gameSession);
            }
            else
            {
                return ReStartLevel(forGameObject);
            }
        }

        private Type ReStartLevel(GameObject forGameObject)
        {
            forGameObject.GetComponent<Level>().Pacman.StopSpinAnimation();

            // Remove the bonus from the level if it exists.
            Bonus bonus = GameObject.FindObjectOfType<Bonus>();
            if (bonus != null)
            {
                GameObject.Destroy(bonus.gameObject);
            }

            // Re-show ghosts and reset their state.
            GhostManager ghostManager = forGameObject.GetComponent<Level>().GhostManager;
            ghostManager.SetGhostsVisibility(true);
            ghostManager.ResetGhostStates();

            return typeof(GetReadyState);
        }

        private Type EndGame(GameSession gameSession)
        {
            // If this is a demo game, then don't bother with the game over state/sequence, just end the scene now.
            if (gameSession is DemoGameSession)
            {
                gameSession.GameOver();
                return GetType();
            }

            return typeof(GameOverState);
        }

        private void StartPacmanSpin(GameObject forGameObject)
        {
            Level level = forGameObject.GetComponent<Level>();
            level.Arena.GetComponent<GreyoutSprite>().GreyEnabled(false);
            level.GhostManager.SetGhostsVisibility(false);
            level.Pacman.StartSpinAnimation();
            UnityEngine.Object.FindObjectOfType<FrightenSiren>().Stop();
        }
    }
}
