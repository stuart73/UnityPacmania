using System;
using UnityEngine;
using Pacmania.Audio;
using Pacmania.Utilities.Sprites;
using Pacmania.Utilities.StateMachines;
using Pacmania.GameManagement;
using Pacmania.InGame.Characters.Ghost;

namespace Pacmania.InGame.LevelStates
{
    public class WinState : BaseState
    {
        private int frame = 0;
        private const int secondsInWinningState = 2;
        private int bonusStep = 0;
        private const int bonusStepDivider = 100;

        public override void OnStateEnter(GameObject forGameObject)
        {
            frame = 0; 

            Level level = forGameObject.GetComponent<Level>();
            level.CharacterManager.PauseCharacters();
            level.Hud.SetLevelCompleteTextVisibility(true);

            // In case ghosts are frighten, stop siren.
            UnityEngine.Object.FindObjectOfType<FrightenSiren>().Stop();

            // If there is a courage bonus set, then enable that and play the jingle.
            if (Game.Instance.CurrentSession is PlayerGameSession session && session.CourageBonus != 0)
            {
                level.AudioManager.PlayAndFade(SoundType.CourageBonusJingle);
                bonusStep = session.CourageBonus / bonusStepDivider;
                level.Hud.SetCourageBonusVisibility(true);
            }

            level.AudioManager.PlayAndFade(SoundType.Clap);
            level.AudioManager.FadeOut(level.Arena.Music); 
        }

        public override Type Update(GameObject forGameObject)
        {
            frame++;

            Level level = forGameObject.GetComponent<Level>();

            // Alternate each frame with greyed out effect (makes is flash).
            GreyoutSprite arneaGreyoutSpriteComponent = level.Arena.GetComponent<GreyoutSprite>();       
            if ((frame % 2) == 0)
            {
                arneaGreyoutSpriteComponent.GreyEnabled(false);
            }
            else
            {
                arneaGreyoutSpriteComponent.GreyEnabled(true);
            }

            if (Game.Instance.CurrentSession is PlayerGameSession session && session.CourageBonus > 0)
            {
                session.AddScore(bonusStep);
                session.CourageBonus -= bonusStep;
            }
            else if (frame >= Game.FramesPerSecond * secondsInWinningState)
            {
                // This will end the scene and start the next level if there is one.
                Game.Instance.CurrentSession.CompletedLevel();
            }

            return GetType();
        }
    }
}
