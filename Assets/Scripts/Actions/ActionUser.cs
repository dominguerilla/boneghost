using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mango.Actions
{
    public class ActionUser : MonoBehaviour
    {
        FPSControls controls;
        public PlayerAction[] actions;

        private void Start()
        {
            controls = new FPSControls();
            foreach (PlayerAction action in actions)
            {
                action.Register(controls); 
            }
            controls.Player.Enable();

        }

        private void Update()
        {
            
        }
    }
}

