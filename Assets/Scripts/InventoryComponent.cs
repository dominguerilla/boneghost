using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour
{
    List<GameObject> inventory;
    
    // Start is called before the first frame update
    void Start()
    {
        inventory = new List<GameObject>();
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
}
