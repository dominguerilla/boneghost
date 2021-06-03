using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mango.Actions
{
    public class ActionUser : MonoBehaviour
    {
        public FPSControls controls;
        public PlayerAction[] actions;

        private void Awake()
        {
            foreach (PlayerAction action in actions)
            {
                action.Register(controls); 
            }
        }
    }
}

