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
        public UnityEvent onAttackStart = new UnityEvent();
        public UnityEvent onAttackEnd = new UnityEvent();

        FPSMovement movementControl;
        FPSLook lookControl;
        FPSDodge dodgeControl;

        private void Awake()
        {
            controls = new FPSControls();
        }

        private void Start()
        {
            foreach (PlayerAction action in actions)
            {
                action.Register(controls);
                if (!movementControl && action is FPSMovement) movementControl = (FPSMovement)action;
                if (!lookControl && action is FPSLook) lookControl = (FPSLook)action;
                if (!dodgeControl && action is FPSDodge) dodgeControl = (FPSDodge)action;
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

        public void EnableSprint()
        {
            movementControl.EnableSprint();
        }

        public void DisableSprint()
        {
            movementControl.DisableSprint();
        }

        public void EnableMouseLook()
        {
            lookControl.EnableMouseLook();
        }

        public void DisableMouseLook()
        {
            lookControl.DisableMouseLook();
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

        public void NotifySprintStart()
        {
            CustomEvent.Trigger(gameObject, "OnSprintStart");
        }

        public void OnSprintStart()
        {
            onSprintStart.Invoke();
        }

        public void OnSprintEnd()
        {
            onSprintEnd.Invoke();
        }

        public void StopSprinting()
        {
            movementControl.StopSprinting();
        }

        public bool CanDodge()
        {
            return dodgeControl.CanDodge();
        }

    }
}

