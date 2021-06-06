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
                controller.Move(moveVector * moveSpeed * Time.deltaTime);
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
            moveVector = transform.TransformDirection(moveVector);

            //Debug.Log($"moveVector: {moveVector}");
            isMoving = true;
        }

        void StopMoving()
        {
            isMoving = false;

        }

        
    }
}

