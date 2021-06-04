using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://github.com/SK-Studios/FPS-with-New-Input-System

namespace Mango.Actions 
{
    public class FPSLook : PlayerAction
    {
        [SerializeField] float sensitivityX = 8f;
        [SerializeField] float sensitivityY = 0.5f;
        float mouseX, mouseY;

        [SerializeField] Transform playerCamera;
        [SerializeField] float xClamp = 85f;
        
        float xRotation = 0f;
        FPSControls controls;

        private void Start()
        {
            LockCursor();
        }

        private void Update()
        {

        }

        public void ReceiveInput(Vector2 mouseInput)
        {
            mouseX = mouseInput.x * sensitivityX;
            mouseY = mouseInput.y * sensitivityY;

            transform.Rotate(Vector3.up, mouseX * Time.deltaTime);

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
            Vector3 targetRotation = transform.eulerAngles;
            targetRotation.x = xRotation;
            playerCamera.eulerAngles = targetRotation;
        }

        public override void Register(FPSControls controls)
        {
            base.Register(controls);
            controls.Player.Look.performed += ctx => this.ReceiveInput(ctx.ReadValue<Vector2>());
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

