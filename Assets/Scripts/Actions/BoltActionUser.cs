using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Bolt;

namespace Mango.Actions
{
    /// <summary>
    /// This class is a unified interface used to control PlayerActions from Bolt Flow/State graphs.
    /// </summary>
    public class BoltActionUser : MonoBehaviour
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
        ArmFighter armControl;
        MenuActions menuControl;

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
                if (!armControl && action is ArmFighter) armControl = (ArmFighter)action;
                if (!menuControl && action is MenuActions) menuControl = (MenuActions)action;

            }
            controls.Player.Enable();

        }

        protected void OnConversationStart(Transform actor)
        {
            DisableMovement();
            DisableMouseLook();
            DisableAttack();
            DisableDodge();
            DisablePause();
            UnlockCursor();
        }

        protected void OnConversationEnd(Transform actor)
        {
            EnableMovement();
            EnableMouseLook();
            EnableAttack();
            EnableDodge();
            EnablePause();
            LockCursor();
        }

        public void NotifyEvent(string eventName)
        {
            CustomEvent.Trigger(this.gameObject, eventName);
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



        public void StartSprint()
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

        public bool CanFight()
        {
            return armControl.CanFight();
        }

        public void EnableAttack()
        {
            armControl.SetCanFight(true);
        }

        public void DisableAttack()
        {
            armControl.SetCanFight(false);
        }

        public void EnablePause()
        {
            menuControl.SetCanPause(true);
        }

        public void DisablePause()
        {
            menuControl.SetCanPause(false);
        }

        public void EnableDodge()
        {
            dodgeControl.SetCanDodge(true);
        }

        public void DisableDodge()
        {
            dodgeControl.SetCanDodge(false);
        }

        public void LockCursor()
        {
            lookControl.LockCursor();
        }

        public void UnlockCursor()
        {
            lookControl.UnlockCursor();
        }

    }
}

