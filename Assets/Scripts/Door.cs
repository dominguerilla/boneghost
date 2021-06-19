using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Key unlockingKey;
    public Vector3 unlockedPosition;
    public Quaternion unlockedRotation;

    Vector3 startingPosition;
    Quaternion startingRotation;

    // Start is called before the first frame update
    void Start()
    {
        this.startingPosition = transform.position;
        this.startingRotation = transform.rotation;
    }

    public void Unlock()
    {
        this.transform.position = unlockedPosition;
        this.transform.rotation = unlockedRotation;
        Debug.Log($"{gameObject.name} unlocked!");
    }

    public void Lock()
    {
        this.transform.position = startingPosition;
        this.transform.rotation = startingRotation;
        Debug.Log($"{gameObject.name} locked.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Colliding with {collision.gameObject.name}");
        Key key = collision.transform.GetComponent<Key>();
        if (key && unlockingKey == key)
        {
            Unlock();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger collision with {other.gameObject.name}");
        Arm arm = other.GetComponent<Arm>();
        if (!arm) return;
        
        Key key = other.transform.GetComponentInChildren<Key>();
        if (key && unlockingKey == key)
        {
            Unlock();
        }
    }
}
