using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mango.Actions
{
    /// <summary>
    /// This class is used to modify/enable/disable player functionality through a single API. 
    /// Things like disabling mouselook, limiting camera rotation, changing movement speed, etc.
    /// </summary>
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

