using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Mango.Actions
{
    /// <summary>
    /// Used to trigger a 'dodge'.
    /// </summary>
    [RequireComponent(typeof(FPSMovement))]
    public class FPSDodge : PlayerAction
    {
        [SerializeField] float dodgeSpeed = 5f;
        [SerializeField] float dodgeTime = 0.125f;
        [SerializeField] float dodgeCooldown = 1.0f;
        public UnityEvent onDodgeStart = new UnityEvent();
        public UnityEvent onDodgeEnd = new UnityEvent();

        CharacterController controller;
        bool canDodge = true;
        FPSMovement movementControl;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            movementControl = GetComponent<FPSMovement>();
        }

        public override void Register(FPSControls controls)
        {
            base.Register(controls);
            controls.Player.Dodge.started += InvokeDodgeStart;
            controls.Player.Dodge.performed += StartDodge;
            controls.Player.Dodge.canceled += StopDodging;
        }

        public bool CanDodge()
        {
            return canDodge;
        }

        public void SetCanDodge(bool value)
        {
            canDodge = value;
        }

        void InvokeDodgeStart(InputAction.CallbackContext ctx)
        {
            if (canDodge) onDodgeStart.Invoke();
        }

        void StartDodge(InputAction.CallbackContext ctx)
        {
            if(canDodge) StartCoroutine(Dodge());
        }

        IEnumerator Dodge()
        {
            canDodge = false;
            Vector3 moveDirection = movementControl.GetMovementVector();
            float ctr = 0;
            while (ctr < dodgeTime)
            {
                controller.Move(moveDirection * dodgeSpeed * Time.deltaTime);
                ctr += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            // dumb hack I have to do so I can call the StopDodging() function because CallbackContexts aren't nullable
            StopDodging(new InputAction.CallbackContext());
            yield return new WaitForSeconds(dodgeCooldown);
            canDodge = true;
        }

        void StopDodging(InputAction.CallbackContext ctx) {
            onDodgeEnd.Invoke();
        }

        private void OnDestroy()
        {
            
            controls.Player.Dodge.started -= InvokeDodgeStart;
            controls.Player.Dodge.performed -= StartDodge;
            controls.Player.Dodge.canceled -= StopDodging;
            
        }
    }
}

