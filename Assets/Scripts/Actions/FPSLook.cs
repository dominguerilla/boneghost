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

        bool _canLook = true;

        private void Start()
        {
            LockCursor();
            InitializeMenuControls();
        }

        private void Update()
        {
            if (_canLook)
            {
                transform.Rotate(Vector3.up, mouseX);

                Vector3 targetRotation = transform.eulerAngles;
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
                targetRotation.x = xRotation;
                playerCamera.eulerAngles = targetRotation;
            }

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

        private void OnPause()
        {
            _canLook = false;
            UnlockCursor();
        }

        private void OnUnpause()
        {
            _canLook = true;
            LockCursor();
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

        private void InitializeMenuControls()
        {
            MenuActions menu = GetComponent<MenuActions>();
            if (menu)
            {
                menu.onPause.AddListener(OnPause);
                menu.onUnpause.AddListener(OnUnpause);
            }
        }
    }
}

