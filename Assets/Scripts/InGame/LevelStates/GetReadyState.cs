using System;
using UnityEngine;
using Pacmania.Audio;
using Pacmania.Utilities.StateMachines;
using Pacmania.InGame.Characters;
using Pacmania.InGame.UI;
using Pacmania.GameManagement;

namespace Pacmania.InGame.LevelStates
{
    public class GetReadyState : BaseState
    {
        private int frame = 0;
        private const int secondsInReadyState = 1;

        public override void OnStateEnter(GameObject forGameObject)
        {
            frame = 0;

            Level level = forGameObject.GetComponent<Level>();
            CharacterManager characterManager = level.CharacterManager;
            characterManager.ResetCharactersToStartPositions();
            characterManager.PauseCharacters();
            Hud hud = level.Hud;
            bool playerSession = Game.Instance.CurrentSession is PlayerGameSession;

            if (playerSession == true)
            {
                hud.SetPlayerReadyTextVisibility(true);
            }
            else
            {
                hud.SetGameOverTextVisibility(true);
                hud.SetPlayerReadyTextVisibility(false);
            }
            hud.RedrawLivesLeft();

            // Start the music if the player is playing this game.
            if (playerSession == true)
            {
                level.AudioManager.Play(level.Arena.Music);
            }
        }

        public override Type Update(GameObject forGameObject)
        {
            frame++;

            //  Do we need to go into Playing state. If so also remove 'player ready' from hud.
            if (frame >= Game.FramesPerSecond * secondsInReadyState)
            {
                forGameObject.GetComponent<Level>().Hud.SetPlayerReadyTextVisibility(false);

                return typeof(PlayingState);
            }
            return GetType();
        }
    }
}
