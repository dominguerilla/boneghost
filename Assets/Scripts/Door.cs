using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Key unlockingKey;
    public Vector3 unlockedPositionOffset;
    public Vector3 unlockedRotation;

    Vector3 startingPosition;
    Quaternion startingRotation;
    bool _isLocked = true;

    // Start is called before the first frame update
    void Start()
    {
        this.startingPosition = transform.position;
        this.startingRotation = transform.rotation;
    }

    public void Unlock()
    {
        this.transform.position += unlockedPositionOffset;
        this.transform.Rotate(unlockedRotation);
        _isLocked = false;
        Debug.Log($"{gameObject.name} unlocked!");
    }

    public void Lock()
    {
        this.transform.position = startingPosition;
        this.transform.rotation = startingRotation;
        _isLocked = true;
        Debug.Log($"{gameObject.name} locked.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Colliding with {collision.gameObject.name}");
        Key key = collision.transform.GetComponent<Key>();
        if (_isLocked && key && unlockingKey == key)
        {
            Unlock();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger collision with {other.gameObject.name}");
        
        Key key = other.transform.GetComponentInChildren<Key>();
        if (_isLocked && key && unlockingKey == key)
        {
            Unlock();
        }
    }

    public void ToggleLock()
    {
        if (_isLocked) Unlock();
        else Lock();
    }
}
