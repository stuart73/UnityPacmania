using System;
using UnityEngine;
using Pacmania.Utilities.StateMachines;

namespace Pacmania.InGame.LevelStates
{
    public class PlayingState : BaseState
    {
        public override void OnStateEnter(GameObject forGameObject)
        {
            forGameObject.GetComponent<Level>().CharacterManager.ResumeCharacters();
        }

        public override Type Update(GameObject forGameObject)
        {
            return GetType();
        }
    }
}
