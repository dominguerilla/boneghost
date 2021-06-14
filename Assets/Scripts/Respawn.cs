using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Respawn : MonoBehaviour
{
    public UnityEvent onRespawn;
    public float respawnHeight = 500;
    public float respawnDepth = 500;
    [SerializeField]
    Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = this.transform.position;
    }

    private void FixedUpdate()
    {
        if (transform.position.y < startingPosition.y - respawnDepth)
        {
            RespawnObject();
        }
    }

    public void RespawnObject()
    {
        CharacterController cc = GetComponent<CharacterController>();
        if(cc) cc.enabled = false;
        transform.position = new Vector3(startingPosition.x, startingPosition.y + respawnHeight, startingPosition.z);
        if(cc) cc.enabled = true;
        onRespawn.Invoke();
    }
}
