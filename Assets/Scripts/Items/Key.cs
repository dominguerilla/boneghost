using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : ItemComponent
{
    public override void Interact(Arm arm, InventoryComponent inventory)
    {
        inventory.Add(this.gameObject);
    }
}
