using System;
using UnityEngine;
using Pacmania.Utilities.StateMachines;

namespace Pacmania.InGame.LevelStates
{
    public class EndState : BaseState
    {
        public override Type Update(GameObject forGameObject)
        {
            return GetType();
        }
    }
}
