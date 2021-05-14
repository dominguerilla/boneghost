using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hazard : MonoBehaviour
{
    public bool isActivated;
    public UnityEvent onActivation = new UnityEvent();
    public UnityEvent onDeactivation = new UnityEvent();

    private void Start()
    {
        if (isActivated) onActivation.Invoke();
    }

    public void Activate()
    {
        if (isActivated) return;
        isActivated = true;
        onActivation.Invoke();
    }

    public void Deactivate()
    {
        if (!isActivated) return;
        isActivated = false;
        onDeactivation.Invoke();
    }

    public void ActivateAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void DeactivateAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated) return;
        Respawnable respawnable = other.GetComponent<Respawnable>();
        if (respawnable)
        {
            respawnable.RespawnObject();
        }
    }
}
