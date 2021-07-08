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
        [SerializeField] float dodgeTime = 1.0f;
        public UnityEvent onDodgeStart = new UnityEvent();
        public UnityEvent onDodgeEnd = new UnityEvent();

        CharacterController controller;
        bool _canDodge = true;
        bool _isMoving = false;
        FPSMovement movementControl;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            movementControl = GetComponent<FPSMovement>();
        }

        public override void Register(FPSControls controls)
        {
            base.Register(controls);
            controls.Player.Dodge.started += ctx => onDodgeStart.Invoke();
            controls.Player.Dodge.performed += ctx => { StartCoroutine(Dodge()); };
            controls.Player.Dodge.canceled += ctx => { this.StopMoving();  onDodgeEnd.Invoke(); };
        }

        IEnumerator Dodge()
        {
            Vector3 moveDirection = movementControl.GetMovementVector();
            _isMoving = true;
            float ctr = 0;
            while (ctr < dodgeTime)
            {
                controller.Move(moveDirection * dodgeSpeed * Time.deltaTime);
                ctr += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        void StopMoving() {
            _isMoving = false;
        }
    }
}

