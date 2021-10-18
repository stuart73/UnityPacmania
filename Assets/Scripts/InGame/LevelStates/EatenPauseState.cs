using System;
using UnityEngine;
using Pacmania.Utilities.StateMachines;
using Pacmania.GameManagement;

namespace Pacmania.InGame.LevelStates
{
    public class EatenPauseState : BaseState
    {
        private int frame = 0;
        private const int secondsInEatenPauseState = 1;

        public override void OnStateEnter(GameObject forGameObject) 
        {
            frame = 0;
            forGameObject.GetComponent<Level>().CharacterManager.PauseCharacters();
        }
        public override Type Update(GameObject forGameObject)
        {
            frame++;
            if (frame >= Game.FramesPerSecond * secondsInEatenPauseState)
            {
                return typeof(PlayingState);
            }
            return GetType();
        }
    }
}
