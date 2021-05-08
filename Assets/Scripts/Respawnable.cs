using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Respawnable : MonoBehaviour
{
    public UnityEvent onRespawn;
    public float respawnHeight = 500;
    public float respawnDepth = 500;
    public bool changeRespawnOnContact = false;

    [SerializeField]
    RespawnPoint respawnPoint;
    [SerializeField]
    Vector3 respawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (respawnPoint)
        {
            SetRespawnPoint(respawnPoint);
        }
        else
        {
            respawnPosition = this.transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.y < respawnPosition.y - respawnDepth)
        {
            RespawnObject(0, respawnHeight, 0);
        }
    }

    public void RespawnObject(float x, float y, float z)
    {
        CharacterController cc = GetComponent<CharacterController>();
        if (cc) cc.enabled = false;
        transform.position = new Vector3(respawnPosition.x + x, respawnPosition.y + y, respawnPosition.z + z);
        if (cc) cc.enabled = true;
        onRespawn.Invoke();
    }

    public void RespawnObject()
    {
        RespawnObject(0, 1f, 0);
    }

    public void SetRespawnPoint(RespawnPoint newRespawnPoint)
    {
        if(respawnPoint) respawnPoint.DeactivatePoint();
        respawnPoint = newRespawnPoint;
        respawnPosition = respawnPoint.GetRespawnPosition();
        respawnPoint.ActivatePoint();
    }

    public RespawnPoint GetRespawnPoint()
    {
        return respawnPoint;
    }
}