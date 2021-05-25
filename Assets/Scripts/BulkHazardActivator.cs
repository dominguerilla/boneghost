using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulkHazardActivator : MonoBehaviour
{
    public void ActivateAllChildHazards()
    {
        foreach (Transform child in transform)
        {
            Hazard hazard = child.GetComponent<Hazard>();
            if (hazard)
            {
                hazard.Activate();
            }
        }
    }
}
