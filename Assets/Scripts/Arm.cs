using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Arm : MonoBehaviour
{
    public Transform heldItemPosition;
    public Vector3 offset;
    public Vector3 eulerOffset;

    public UnityEvent onItemUseStart = new UnityEvent();
    public UnityEvent onItemUseEnd = new UnityEvent();
    public UnityEvent onItemEquip = new UnityEvent();
    public UnityEvent onItemDequip = new UnityEvent();

    [SerializeField] ItemComponent heldItem;
    [SerializeField] Animator armAnimator;
    [SerializeField] string animTriggerPrefix = "";
    [SerializeField] WeaponUI ui;


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
        heldItem.onUseStart.AddListener(onItemUseStart.Invoke);
        heldItem.onUseEnd.AddListener(onItemUseEnd.Invoke);
        onItemEquip.Invoke();
        if (ui && item is Weapon) ui.RegisterWeapon((Weapon)item);
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
            heldItem.onUseStart.RemoveListener(onItemUseStart.Invoke);
            heldItem.onUseEnd.RemoveListener(onItemUseEnd.Invoke);
            heldItem = null;
            onItemDequip.Invoke();
        }
    }

    public void DropIfTemporary(Vector3 location)
    {
        if (heldItem && heldItem.isTemporary)
        {
            Drop(location);
        }
    }

    public void TriggerAnimation(string triggerName)
    {
        if (armAnimator)
        {
            armAnimator.SetTrigger(animTriggerPrefix + triggerName);
        }
        else
        {
            Debug.LogWarning($"No arm animator set for { gameObject.name }!");
        }
    }

    public bool IsHoldingItem()
    {
        return heldItem != null;
    }
}
