using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Actions
{
    public class PlayerAction : MonoBehaviour
    {
        FPSControls controls;

        public void Register(FPSControls controls)
        {
            this.controls = controls;
            Debug.Log($"{ this.GetType().Name } controls registered on {gameObject.name}!");
        }
    }
}

