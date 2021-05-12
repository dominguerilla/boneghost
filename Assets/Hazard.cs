using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Respawnable respawnable = other.GetComponent<Respawnable>();
        if (respawnable)
        {
            respawnable.RespawnObject();
        }
    }
}
