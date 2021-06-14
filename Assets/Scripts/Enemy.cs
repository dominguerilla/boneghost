using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody[] rigidbodies;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        DisableRigidbodies();
    }

    void DisableRigidbodies()
    {
        foreach (Rigidbody body in rigidbodies)
        {
            body.isKinematic = true;
        }
    }
}
