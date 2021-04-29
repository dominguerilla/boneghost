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

    Vector3 _previousPosition;

    private void Start()
    {
        _previousPosition = referenceRotation.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTransform();
    }

    void UpdateTransform()
    {
        Vector3 currentPosition = referenceRotation.position;
        if (currentPosition != this._previousPosition)
        {
            this.transform.position += (currentPosition - _previousPosition) * (1 / scaleFactor);
            this._previousPosition = referenceRotation.position;
        }
        this.transform.eulerAngles = referenceRotation.eulerAngles;


    }
}
