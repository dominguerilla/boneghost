using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Requires a target transform. When Play() is invoked, the attached transform will relocate to the target's
/// position and invoke all listeners.
/// 
/// Useful for emitting particles at a position of a moving object without parenting GameObjects.
/// </summary>
public class FollowAndPlay : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public UnityEvent onPlay = new UnityEvent();

    // Start is called before the first frame update
    void Awake()
    {
        if (!target)
        {
            Debug.LogError($"No target GameObject set for FollowAndPlay { gameObject.name }!");
            Destroy(this);
        }
    }

    public void Play()
    {
        transform.position = target.position + offset;
        onPlay.Invoke();
    }
}
