using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mango.Actions
{
    public class ActionUser : MonoBehaviour
    {
        FPSControls controls;
        public PlayerAction[] actions;

        private void Awake()
        {
            controls = new FPSControls();
        }

        private void Start()
        {
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

