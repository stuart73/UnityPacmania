using System;
using UnityEngine;

namespace Pacmania.Utilities.StateMachines
{
    public abstract class BaseState
    {
        public BaseState()
        {          
        }

        public virtual void OnStateEnter(GameObject forGameObject) { }
        public virtual void OnStateLeave(GameObject forGameObject) { }
        public abstract Type Update(GameObject forGameObject);
    }
}
