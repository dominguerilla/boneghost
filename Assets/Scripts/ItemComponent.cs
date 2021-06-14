using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class ItemComponent : MonoBehaviour
{
    public UnityEvent onEquip = new UnityEvent();
    public UnityEvent onDequip = new UnityEvent();

    [SerializeField] protected Vector3 targetLocalOrientation;
    [SerializeField] protected Vector3 targetLocalOffset;
    [SerializeField] protected Arm equippedArm;

    /// <summary>
    /// Does the player have to hold down Mouse button to keep a grip on this item?
    /// </summary>
    public bool isTemporary;

    protected Rigidbody rb;
    protected bool isEquipped;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();    
    }

    void Update()
    {
        if (isEquipped)
        {
            whileEquipped();
        }
    }

    /// <summary>
    /// Called on every frame the Item is equipped.
    /// </summary>
    public virtual void whileEquipped() { }

    /// <summary>
    /// Uses the Item.
    /// </summary>
    public virtual void Use() { }
    public virtual void Equip(Arm arm, Transform parentObject, Vector3 offset, Vector3 eulerOffset)
    {
        Freeze();
        this.equippedArm = arm;
        this.transform.SetParent(parentObject);
        Orient(offset, eulerOffset);
        isEquipped = true;
        onEquip.Invoke();
    }

    public virtual void Dequip()
    {
        Unfreeze();
        this.transform.SetParent(null);
        this.equippedArm = null;
        isEquipped = false;
        onDequip.Invoke();
    }
    void Freeze()
    {
        rb.isKinematic = true;
    }

    void Unfreeze()
    {
        rb.isKinematic = false;
    }
    void Orient(Vector3 offset, Vector3 eulerOffset)
    {
        this.transform.localPosition = Vector3.zero;
        this.transform.localEulerAngles = Vector3.zero;

        this.transform.localPosition = targetLocalOffset + offset;
        this.transform.localEulerAngles = targetLocalOrientation + eulerOffset;
    }
}
