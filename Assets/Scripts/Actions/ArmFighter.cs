using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Mango.Actions
{
    // TODO: Move Item Equip/Dequip logic to a separate inventory system (InventoryComponent?)
    public class ArmFighter : PlayerAction
    {
        public UnityEvent onAttackStart = new UnityEvent();
        public UnityEvent onAttackEnd = new UnityEvent();

        [SerializeField] Arm[] arms;
        [SerializeField] Weapon[] startingWeapons;
        [SerializeField] Camera cam;
        [SerializeField] InventoryComponent inventory;
        [SerializeField] Animator armAnim;

        bool canFight = true;

        public override void Register(FPSControls controls)
        {
            base.Register(controls);
            controls.Player.Interact1.performed += _ => StartAttack(0);
            controls.Player.Interact2.performed += _ => StartAttack(1);
        }

        private void Awake()
        {
            if (!cam)
            {
                cam = Camera.main;
            }
        }

        private void Start()
        {
            EquipItems();
        }

        // TODO: Use a different cursor manager?
        void OnGUI()
        {
            GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 10, 10), "");
        }

        void StartAttack(int armNum)
        {
            if (!canFight || arms.Length <= 0) return;
            Attack(arms[armNum]);
        }

        bool Attack(Arm arm)
        {
            if (arm.IsHoldingItem())
            {
                arm.UseItem();

            }
            return false;
        }

        public Animator GetAnimator()
        {
            return armAnim;
        }

        public bool SetCanFight(bool value)
        {
            canFight = value;
            return canFight;
        }

        public bool CanFight()
        {
            return canFight;
        }

    #region InventoryLogic
        void EquipWeapon(Weapon weapon, Arm arm)
        {
            weapon.Interact(arm, inventory);
            arm.onItemUseStart.AddListener(onAttackStart.Invoke);
            arm.onItemUseEnd.AddListener(onAttackEnd.Invoke);
        }

        public ItemComponent GetItem(int armIndex)
        {
            return arms[armIndex].GetItem();
        }

        void EquipItems()
        {
            if (arms.Length == startingWeapons.Length)
            {
                EquipWeapon(startingWeapons[0], arms[0]);
                EquipWeapon(startingWeapons[1], arms[1]);
            }
            else if (arms.Length > 0 && startingWeapons.Length > 0)
            {
                Debug.LogError("Not enough arms/starting items for item initialization!");
            }
        }
    }
    #endregion
}

