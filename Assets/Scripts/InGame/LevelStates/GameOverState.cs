using System;
using UnityEngine;
using Pacmania.Audio;
using Pacmania.Utilities.Sprites;
using Pacmania.Utilities.StateMachines;
using Pacmania.GameManagement;

namespace Pacmania.InGame.LevelStates
{
    public class GameOverState : BaseState
    {
        private int frame = 0;
        private const int secondsInGameOverState = 4;

        public override void OnStateEnter(GameObject forGameObject)
        {
            frame = 0;

            Level level = forGameObject.GetComponent<Level>();
      
            level.CharacterManager.PauseCharacters();
            level.CharacterManager.HideCharacters();
            level.Arena.GetComponent<GreyoutSprite>().EnableGrey();
            level.Hud.SetGameOverTextVisibility(true);
            level.Hud.SetSeeYouTextVisibility(true);
            level.AudioManager.Play(SoundType.GameOver);
            level.Arena.HideAllPickups();
            level.Hud.StartScreenFadeout();      
        }

        public override Type Update(GameObject forGameObject)
        {
            frame++;
            if (frame >= Game.FramesPerSecond * secondsInGameOverState)
            {
                Game.Instance.CurrentSession.GameOver();
                return typeof(EndState);
            }
            return GetType();
        }
    }
}
