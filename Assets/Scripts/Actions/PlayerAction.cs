using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Actions
{
    public class PlayerAction : MonoBehaviour
    {
        protected FPSControls controls;

        public virtual void Register(FPSControls controls)
        {
            this.controls = controls;
            //Debug.Log($"{ this.GetType().Name } controls registered on {gameObject.name}!");
        }
    }
}

