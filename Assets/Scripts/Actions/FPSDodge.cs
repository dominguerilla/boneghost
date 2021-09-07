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
        [Tooltip("If specified, starts updating the dodge stamina bar image's fill amount gradually.")]
        [SerializeField] ImageFiller dodgeCooldownBar;
        public UnityEvent onDodgeStart = new UnityEvent();
        public UnityEvent onDodgeEnd = new UnityEvent();

        CharacterController controller;
        bool canDodge = true, isCoolingDown = false;
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
            return canDodge && !isCoolingDown;
        }

        public void SetCanDodge(bool value)
        {
            canDodge = value;
        }

        void InvokeDodgeStart(InputAction.CallbackContext ctx)
        {
            if (canDodge && !isCoolingDown) onDodgeStart.Invoke();
        }

        void StartDodge(InputAction.CallbackContext ctx)
        {
            if(canDodge && !isCoolingDown) StartCoroutine(Dodge());
        }

        IEnumerator Dodge()
        {
            isCoolingDown = true;
            Vector3 moveDirection = movementControl.GetMovementVector();
            float ctr = 0;
            if (dodgeCooldownBar) dodgeCooldownBar.ResetAndFillImage(dodgeCooldown + dodgeTime);
            while (ctr < dodgeTime)
            {
                controller.Move(moveDirection * dodgeSpeed * Time.deltaTime);
                ctr += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            // dumb hack I have to do so I can call the StopDodging() function because CallbackContexts aren't nullable
            StopDodging(new InputAction.CallbackContext());
            yield return new WaitForSeconds(dodgeCooldown);
            isCoolingDown = false;
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

