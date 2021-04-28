using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxCamera : MonoBehaviour
{
    /// <summary>
    /// Syncs the rotation of the SkyboxCamera with this transform.
    /// </summary>
    [SerializeField]
    Transform referenceRotation;

    [SerializeField]
    float scaleFactor = 20;

    // Update is called once per frame
    void Update()
    {
        UpdateTransform();
    }

    void UpdateTransform()
    {
        this.transform.eulerAngles = referenceRotation.eulerAngles;
    }
}
