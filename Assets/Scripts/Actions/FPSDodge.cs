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
        bool _canDodge = true;
        FPSMovement movementControl;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            movementControl = GetComponent<FPSMovement>();
        }

        public override void Register(FPSControls controls)
        {
            base.Register(controls);
            controls.Player.Dodge.started += ctx => { if (_canDodge) onDodgeStart.Invoke(); };
            controls.Player.Dodge.performed += ctx => { StartCoroutine(Dodge()); };
            controls.Player.Dodge.canceled += ctx => this.StopDodging();
        }

        public bool CanDodge()
        {
            return _canDodge;
        }

        IEnumerator Dodge()
        {
            if (!_canDodge) yield break;
            _canDodge = false;
            Vector3 moveDirection = movementControl.GetMovementVector();
            float ctr = 0;
            while (ctr < dodgeTime)
            {
                controller.Move(moveDirection * dodgeSpeed * Time.deltaTime);
                ctr += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            StopDodging();
            yield return new WaitForSeconds(dodgeCooldown);
            _canDodge = true;
        }

        void StopDodging() {
            onDodgeEnd.Invoke();
        }
    }
}

