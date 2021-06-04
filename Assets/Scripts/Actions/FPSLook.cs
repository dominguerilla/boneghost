using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://github.com/SK-Studios/FPS-with-New-Input-System

namespace Mango.Actions 
{
    public class FPSLook : PlayerAction
    {
        float mouseX, mouseY;

        [SerializeField] Transform playerCamera;
        [SerializeField] float xClamp = 85f;
        
        float xRotation = 0f;

        private void Start()
        {
            LockCursor();
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, mouseX);

            Vector3 targetRotation = transform.eulerAngles;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
            targetRotation.x = xRotation;
            playerCamera.eulerAngles = targetRotation;
        }

        public void ReceiveInput(Vector2 mouseInput)
        {
            mouseX = mouseInput.x ;
            mouseY = mouseInput.y ;
        }

        public void ResetMouseDelta()
        {
            mouseX = 0;
            mouseY = 0;
        }

        public override void Register(FPSControls controls)
        {
            base.Register(controls);
            controls.Player.Look.performed += ctx => this.ReceiveInput(ctx.ReadValue<Vector2>());
            controls.Player.Look.canceled += _ => this.ResetMouseDelta();
        }

        private void UnlockCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

