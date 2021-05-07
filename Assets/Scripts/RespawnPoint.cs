using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RespawnPoint : MonoBehaviour
{
    public UnityEvent onActivation = new UnityEvent();
    public UnityEvent onDeactivation = new UnityEvent();

    [SerializeField]
    Transform respawnPoint;

    [SerializeField]
    bool isActivated = false;

    public void ActivatePoint()
    {
        if (isActivated) return;
        isActivated = true;
        onActivation.Invoke();
    }

    public void DeactivatePoint()
    {
        if (!isActivated) return;
        isActivated = false;
        onDeactivation.Invoke();
    }

    public Vector3 GetRespawnPosition()
    {
        return respawnPoint.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Respawnable r = other.GetComponent<Respawnable>();
        if (r && r.changeRespawnOnContact)
        {
            r.SetRespawnPoint(this);
        }
    }
}
