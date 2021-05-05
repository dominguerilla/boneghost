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
    Vector3 _startRotation;

    private void Start()
    {
        _previousPosition = referenceRotation.position;
        _startRotation = this.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        //RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyboxRotationSpeed);
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
