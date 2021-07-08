using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Bolt;

namespace Mango.Actions
{
    /// <summary>
    /// This class is used to modify/enable/disable player functionality through a single API. 
    /// Things like disabling mouselook, limiting camera rotation, changing movement speed, etc.
    /// </summary>
    public class PlayerActionUser : MonoBehaviour
    {
        FPSControls controls;
        public PlayerAction[] actions;

        public UnityEvent onMoveStart = new UnityEvent();
        public UnityEvent onMoveEnd = new UnityEvent();
        public UnityEvent onDodgeStart = new UnityEvent();
        public UnityEvent onDodgeEnd = new UnityEvent();
        public UnityEvent onSprintStart = new UnityEvent();
        public UnityEvent onSprintEnd = new UnityEvent();

        FPSMovement movementControl;

        private void Awake()
        {
            controls = new FPSControls();
        }

        private void Start()
        {
            foreach (PlayerAction action in actions)
            {
                action.Register(controls);
                if (action is FPSMovement) movementControl = (FPSMovement)action;
            }
            controls.Player.Enable();

        }

        public void EnableMovement()
        {
            movementControl.UnlockMovement();
        }

        public void DisableMovement()
        {
            movementControl.LockMovement();
        }

        public bool IsMoving()
        {
            return movementControl.IsMoving();
        }

        public void OnMoveStart()
        {
            onMoveStart.Invoke();
        }

        public void OnMoveEnd()
        {
            onMoveEnd.Invoke();
        }

        public void OnDodgeStart()
        {
            onDodgeStart.Invoke();
        }

        public void OnDodgeEnd()
        {
            onDodgeEnd.Invoke();
        }

        public void OnSprintStart()
        {
            onSprintStart.Invoke();
        }

        public void OnSprintEnd()
        {
            onSprintEnd.Invoke();
        }

    }
}

