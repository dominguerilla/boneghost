using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mango.Actions
{
    public class MouseInteract : PlayerAction
    {
        public float maxInteractionDistance = 2f;
        public float maxDropDistance = 2f;
        public float interactionCooldownTime = 1f;

        [SerializeField] Arm[] arms;
        [SerializeField] Camera cam;
        [SerializeField] InventoryComponent inventory;
        [SerializeField] Animator armAnim;
        Vector3[] originalArmPositions;

        Vector3 surfaceUnderCursor;
        Coroutine cooldownRoutine;
        bool interactionCoolingDown;

        public override void Register(FPSControls controls)
        {
            base.Register(controls);
            controls.Player.Interact1.performed += _ => StartInteract(0);
            controls.Player.Interact2.performed += _ => StartInteract(1);
            controls.Player.Interact1.canceled += _ => StopInteract(0);
            controls.Player.Interact2.canceled += _ => StopInteract(1);
            controls.Player.Drop1.performed += _ => DropItem(0);
            controls.Player.Drop2.performed += _ => DropItem(1);
        }

        private void Awake()
        {
            originalArmPositions = new Vector3[arms.Length];
            for (int i = 0; i < arms.Length; i++)
            {
                originalArmPositions[i] = arms[i].transform.localPosition;
            }
            if (!cam)
            {
                cam = Camera.main;
            }
        }

        void OnGUI()
        {
            GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 10, 10), "");
        }

        private void FixedUpdate()
        {
            surfaceUnderCursor = GetSurfaceUnderCursor();
        }

        void StartInteract(int armNum)
        {
            if (arms.Length <= 0) return;
            Vector3 direction = cam.transform.forward;
            ReachArm(armNum, direction);
            UseArm(arms[armNum], Vector3.zero);
        }

        void StopInteract(int armNum)
        {
            if (arms.Length <= 0) return;
            RetractArm(armNum);
        }

        void ReachArm(int armNum, Vector3 direction)
        {
            if (arms.Length <= 0 || arms[armNum].IsHoldingItem()) return;
            //arms[armNum].transform.position += direction;
            if (interactionCoolingDown) return;
            cooldownRoutine = StartCoroutine(InteractionCooldown(interactionCooldownTime));
            string triggerName = armNum % 2 == 0 ? "l_reach" : "r_reach";
            armAnim.SetTrigger(triggerName);
        }

        IEnumerator InteractionCooldown(float time)
        {
            interactionCoolingDown = true;
            yield return new WaitForSeconds(time);
            interactionCoolingDown = false;
        }
         
        void RetractArm(int armNum)
        {
            if (arms.Length <= 0) return;
            //arms[armNum].DropIfTemporary(surfaceUnderCursor);
            //arms[armNum].transform.localPosition = originalArmPositions[armNum];
            string triggerName = armNum % 2 == 0 ? "l_retract" : "r_retract";
            armAnim.SetTrigger(triggerName);
        }

        void DropItem(int armNum)
        {
            if (arms.Length <= 0) return;
            arms[armNum].Drop(surfaceUnderCursor);
        }

        bool UseArm(Arm arm, Vector3 eulerAngleOffset)
        {
            if (arm.IsHoldingItem())
            {
                arm.UseItem();
                return false;
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Pointer.current.position.ReadValue());
            
            if (Physics.Raycast(ray, out hit, maxInteractionDistance, LayerMask.GetMask("Interactable")))
            {
                Transform objectHit = hit.transform;
                ItemComponent item = objectHit.GetComponent<ItemComponent>();
                if (item)
                {
                    item.Interact(arm, inventory);
                    return true;
                }
            }

            return false;
        }

        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying) Debug.DrawLine(cam.transform.position, surfaceUnderCursor, Color.red);
        }

        Vector3 GetSurfaceUnderCursor()
        {
            RaycastHit hit;
            Ray cameraForward = GetCameraForward();
            if (Physics.Raycast(cameraForward, out hit, maxDropDistance,LayerMask.GetMask("Default")))
            {
                return hit.point;
            }
            return GetCameraForwardVector();
        }

        Ray GetCameraForward()
        {
            return cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, maxDropDistance));
        }

        Vector3 GetCameraForwardVector()
        {
            return cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, maxDropDistance));
        }

        public Animator GetAnimator()
        {
            return armAnim;
        }

    }
}

