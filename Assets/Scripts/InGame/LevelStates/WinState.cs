using System;
using UnityEngine;
using Pacmania.Audio;
using Pacmania.Utilities.Sprites;
using Pacmania.Utilities.StateMachines;
using Pacmania.GameManagement;

namespace Pacmania.InGame.LevelStates
{
    public class WinState : BaseState
    {
        private int frame = 0;
        private const int secondsInWinningState = 2;
        private int bonusStep = 0;
        private const int bonusStepDivider = 100;
        private int lastRenderframe = -1;

        public override void OnStateEnter(GameObject forGameObject)
        {
            frame = 0; 

            Level level = forGameObject.GetComponent<Level>();
            level.CharacterManager.PauseCharacters();
            level.Hud.SetLevelCompleteTextVisibility(true);

            // In case ghosts are frighten, stop siren.
            level.FrightenSiren.Stop();

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

            // Alternate each render frame with greyed out effect (makes is flash).
            GreyoutSprite arneaGreyoutSpriteComponent = level.Arena.GetComponent<GreyoutSprite>();

            int renderFrame = Time.frameCount;
            if (renderFrame != lastRenderframe)
            {
                lastRenderframe = renderFrame;
                if (arneaGreyoutSpriteComponent.UsingGreyMateria == true)
                {
                    arneaGreyoutSpriteComponent.DisableGrey();
                }
                else
                {
                    arneaGreyoutSpriteComponent.EnableGrey();
                }
            }

            if (Game.Instance.CurrentSession is PlayerGameSession session && session.CourageBonus > 0)
            {
                session.AddScore(level, bonusStep);
                session.CourageBonus -= bonusStep;
            }
            else if (frame >= Game.FramesPerSecond * secondsInWinningState)
            {
                arneaGreyoutSpriteComponent.DisableGrey();
                // This will end the scene and start the next level if there is one.
                Game.Instance.CurrentSession.CompletedLevel();
                return typeof(EndState);
            }

            return GetType();
        }
    }
}
