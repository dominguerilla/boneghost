using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    public Transform heldItemPosition;
    public Vector3 offset;
    public Vector3 eulerOffset;

    [SerializeField] ItemComponent heldItem;

    private void Awake()
    {
        if (!heldItemPosition)
        {
            heldItemPosition = transform;
        }
    }

    public Transform GetItemPosition()
    {
        return heldItemPosition;
    }

    public void Hold(ItemComponent item)
    {
        heldItem = item;
    }

    public void UseItem()
    {
        if (heldItem)
        {
            heldItem.Use();
        }
        else
        {
            Debug.LogError("Trying to use Item while Arm is empty!");
        }
    }

    public void Drop(Vector3 location)
    {
        if (heldItem)
        {
            heldItem.transform.position = location;
            heldItem.Dequip();
            heldItem = null;
        }
    }

    public void DropIfTemporary(Vector3 location)
    {
        if (heldItem && heldItem.isTemporary)
        {
            Drop(location);
        }
    }

    public bool IsHoldingItem()
    {
        return heldItem != null;
    }
}
