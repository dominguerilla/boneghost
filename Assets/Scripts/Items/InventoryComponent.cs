using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour
{
    public bool isInitialized { get; private set; }

    [SerializeField] Weapon[] startingWeapons;
    [SerializeField] Arm[] arms;
    List<GameObject> inventory;
    Weapon equippedWeaponLeft, equippedWeaponRight;

    // Start is called before the first frame update
    void Start()
    {
        inventory = new List<GameObject>();
        EquipStartingItems();
        isInitialized = true;
    }

    public void Add(GameObject item){
        this.inventory.Add(item);
        item.SetActive(false);
    }

    public void Drop(Vector3 position){
        if(this.inventory.Count > 0){
            GameObject item = inventory[0];
            this.inventory.Remove(item);
            item.transform.position = position;
            item.SetActive(true);
        }
    }

    Weapon EquipWeapon(Weapon weapon, Arm arm)
    {
        weapon.Interact(arm, this);
        return weapon;
    }

    public ItemComponent GetItem(int armIndex)
    {
        return arms[armIndex].GetItem();
    }

    void EquipStartingItems()
    {
        if (arms.Length == startingWeapons.Length)
        {
            equippedWeaponLeft = EquipWeapon(startingWeapons[0], arms[0]);
            equippedWeaponRight = EquipWeapon(startingWeapons[1], arms[1]);
        }
        else if (arms.Length > 0 && startingWeapons.Length > 0)
        {
            Debug.LogError("Not enough arms/starting items for item initialization!");
        }
    }

    public void ApplyStatus(Status status)
    {
        equippedWeaponLeft.ApplyStatus(status);
        equippedWeaponRight.ApplyStatus(status);
    }


}

