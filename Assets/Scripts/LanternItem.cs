using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternItem : ItemComponent
{

    public override void Dequip()
    {
        base.Dequip();
        this.transform.eulerAngles = Vector3.zero;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
