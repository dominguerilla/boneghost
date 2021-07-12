using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Mango.Actions
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSMovement : PlayerAction
    {
        [SerializeField]
        float moveSpeed = 5f;
        [SerializeField]
        float sprintModifier = 1.5f;

        public UnityEvent onMoveStart = new UnityEvent();
        public UnityEvent onMoveEnd = new UnityEvent();
        public UnityEvent onSprintStart = new UnityEvent();
        public UnityEvent onSprintEnd = new UnityEvent();

        CharacterController controller;
        Vector3 moveVector;

        bool _canMove = true;
        bool _canSprint = false;
        bool _isMoving = false;
        bool _isSprinting = false;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            InitializeMenuControls();
        }

        private void Update()
        {
            if (_canMove && _isMoving)
            {
                Vector3 moveDirection = CalculateMoveDirection(moveVector);
                float speed = _canSprint && _isSprinting ? moveSpeed * sprintModifier : moveSpeed;
                controller.Move(moveDirection * speed * Time.deltaTime);
            }
            
        }

        public override void Register(FPSControls controls)
        {
            base.Register(controls);
            controls.Player.Move.started += ctx => onMoveStart.Invoke();
            controls.Player.Move.performed += ctx => ReceiveInput(ctx.ReadValue<Vector2>());
            controls.Player.Move.canceled += _ => StopMoving();
            controls.Player.Sprint.performed += _ => StartSprinting();
            controls.Player.Sprint.canceled += _ => StopSprinting();
        }

        public void LockMovement() {
            _canMove = false;
        }

        public void UnlockMovement()
        {
            _canMove = true;
        }

        public void EnableSprint()
        {
            _canSprint = true;
        }

        public void DisableSprint()
        {
            _canSprint = false;
        }

        public void StartSprinting()
        {
            if (_canSprint)
            {
                onSprintStart.Invoke(); 
                _isSprinting = true;
            }
        }

        public void StopSprinting()
        {
            onSprintEnd.Invoke(); 
            _isSprinting = false;
        }

        void StopMoving()
        {
            _isMoving = false;
            StopSprinting();
            onMoveEnd.Invoke();
        }

        public void ReceiveInput(Vector2 compositeInput)
        {
            moveVector = new Vector3(
                compositeInput.x, 
                0, 
                compositeInput.y
            );

            //Debug.Log($"moveVector: {moveVector}");
            _isMoving = true;
        }

        public Vector3 GetMovementVector()
        {
            return _isMoving ? CalculateMoveDirection(moveVector) : transform.forward;
        }

        public bool IsMoving()
        {
            return _isMoving;
        }

        public bool CanSprint()
        {
            return _canSprint;
        }


        Vector3 CalculateMoveDirection(Vector3 inputVector)
        {
            Vector3 forwardDir = transform.forward * inputVector.z;
            Vector3 sideDir = transform.right * inputVector.x;
            return (forwardDir + sideDir).normalized;
        }

        void InitializeMenuControls()
        {
            MenuActions menu = GetComponent<MenuActions>();
            if (menu)
            {
                menu.onPause.AddListener(LockMovement);
                menu.onUnpause.AddListener(UnlockMovement);
            }
        }

        
    }
}

