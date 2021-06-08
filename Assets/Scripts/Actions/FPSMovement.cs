using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Actions
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSMovement : PlayerAction
    {
        [SerializeField]
        float moveSpeed = 5f;

        CharacterController controller;
        Vector3 moveVector;

        bool _canMove = true;
        bool _isMoving = false;

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

                controller.Move(moveDirection * moveSpeed * Time.deltaTime);
            }
            
        }

        public override void Register(FPSControls controls)
        {
            base.Register(controls);
            controls.Player.Move.performed += ctx => this.ReceiveInput(ctx.ReadValue<Vector2>());
            controls.Player.Move.canceled += _ => this.StopMoving();
        }

        public void LockMovement() {
            _canMove = false;
        }

        public void UnlockMovement()
        {
            _canMove = true;
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

        Vector3 CalculateMoveDirection(Vector3 inputVector)
        {
            Vector3 forwardDir = transform.forward * inputVector.z;
            Vector3 sideDir = transform.right * inputVector.x;
            return (forwardDir + sideDir).normalized;
        }

        void StopMoving()
        {
            _isMoving = false;

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

