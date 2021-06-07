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

        bool isMoving = false;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            Debug.Log($"{gameObject.name}.transform.forward: {transform.forward}");
        }

        private void Update()
        {
            if (isMoving)
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

        public void ReceiveInput(Vector2 compositeInput)
        {
            moveVector = new Vector3(
                compositeInput.x, 
                0, 
                compositeInput.y
            );

            //Debug.Log($"moveVector: {moveVector}");
            isMoving = true;
        }

        Vector3 CalculateMoveDirection(Vector3 inputVector)
        {
            Vector3 forwardDir = transform.forward * inputVector.z;
            Vector3 sideDir = transform.right * inputVector.x;
            return (forwardDir + sideDir).normalized;
        }

        void StopMoving()
        {
            isMoving = false;

        }

        
    }
}

