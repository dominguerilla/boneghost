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
    Vector3 respawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        respawnPosition = this.transform.position;
    }

    private void FixedUpdate()
    {
        if (transform.position.y < respawnPosition.y - respawnDepth)
        {
            RespawnObject();
        }
    }

    public void RespawnObject()
    {
        CharacterController cc = GetComponent<CharacterController>();
        if (cc) cc.enabled = false;
        transform.position = new Vector3(respawnPosition.x, respawnPosition.y + respawnHeight, respawnPosition.z);
        if (cc) cc.enabled = true;
        onRespawn.Invoke();
    }
}