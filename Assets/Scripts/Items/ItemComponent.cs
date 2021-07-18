using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class ItemComponent : MonoBehaviour
{
    public UnityEvent onEquip = new UnityEvent();
    public UnityEvent onDequip = new UnityEvent();
    public UnityEvent onUseStart = new UnityEvent();
    public UnityEvent onUseEnd = new UnityEvent();

    [SerializeField] AudioClip onUseClip;

    [SerializeField] protected Vector3 targetLocalEuler;
    [SerializeField] protected Vector3 targetLocalOffset;
    [SerializeField] protected Arm equippedArm;

    /// <summary>
    /// Does the player have to hold down Mouse button to keep a grip on this item?
    /// </summary>
    public bool isTemporary;

    protected Rigidbody rb;
    protected Collider col;
    protected AudioSource audioSource;
    protected bool isEquipped;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
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
        arm.Hold(this);
        onEquip.Invoke();
    }

    public virtual void Dequip()
    {
        Unfreeze();
        this.transform.SetParent(null);
        this.transform.eulerAngles = Vector3.zero;
        this.equippedArm = null;
        isEquipped = false;
        
        onDequip.Invoke();
    }

    public virtual void Interact(Arm arm, InventoryComponent inventory)
    {
        Equip(arm, arm.heldItemPosition, arm.offset, arm.eulerOffset);
    }

    public virtual void PlayOnUseSound(float delay)
    {
        if (audioSource)
        {
            audioSource.PlayOneShot(onUseClip);
        }
        else
        {
            Debug.LogError($"No Audiosource on item { gameObject.name }!");
        }
    }

    void Freeze()
    {
        col.isTrigger = true;
        rb.isKinematic = true;
    }

    void Unfreeze()
    {
        col.isTrigger = false;
        rb.isKinematic = false;
    }
    void Orient(Vector3 offset, Vector3 eulerOffset)
    {
        this.transform.localPosition = Vector3.zero;
        this.transform.localEulerAngles = Vector3.zero;

        this.transform.localPosition = targetLocalOffset + offset;
        this.transform.localEulerAngles = targetLocalEuler + eulerOffset;
    }
}
