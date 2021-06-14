using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Vision : MonoBehaviour
{
    public UnityEvent OnDetectPlayer = new UnityEvent();
    public UnityEvent OnLosePlayer = new UnityEvent();

    [SerializeField] Transform eyePosition;
    GameObject seenTarget;

    bool sawPlayer = false;

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log($"{other.gameObject.name} entered field of vision.");
        if (other.gameObject.tag == "Player")
        {
            if (CanSee(other.gameObject))
            {
                seenTarget = other.gameObject;
                sawPlayer = true;
                OnDetectPlayer.Invoke();
            }
            else if (sawPlayer)
            {
                LosePlayer();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log($"{other.gameObject.name} exited field of vision.");
        if (other.gameObject.tag == "Player")
        {
            if (sawPlayer)
            {
                LosePlayer();
            }
        }
    }

    void LosePlayer()
    {
        sawPlayer = false;
        seenTarget = null;
        OnLosePlayer.Invoke();
    }

    bool CanSee(GameObject other)
    {
        RaycastHit hit;
        if (Physics.Raycast(eyePosition.position, other.transform.position - eyePosition.position, out hit)) {
            if (hit.transform.tag == "Player")
            {
                return true;
            }
        }
        return false;
        
    }

    public GameObject GetSeenTarget()
    {
        return seenTarget;
    }
}
