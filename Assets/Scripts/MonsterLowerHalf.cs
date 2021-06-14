using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterLowerHalf : Monster
{
    [SerializeField] MonsterHead head;
    public UnityEvent onDetach = new UnityEvent();

    bool _isHeadDetached = false;
    bool _isGarlicked = false;
    bool _isSalted = false;

    public override void Attack()
    {
        Detach();
    }

    public override void OnSalted()
    {
        if (_isHeadDetached)
        {
            _isSalted = true;
            Debug.Log($"{this.gameObject.name} has been salted!");
            if (_isGarlicked)
            {
                SelfDestruct(1.0f);
            }
            return;
        }

        base.OnSalted();
        
    }

    public override void OnGarlicked()
    {
        if (_isHeadDetached)
        {
            _isGarlicked = true;
            Debug.Log($"{this.gameObject.name} has been garlicked!");
            if (_isSalted)
            {
                SelfDestruct(1.0f);
            }
            return;
        }

        base.OnGarlicked();
        
    }

    public void Detach()
    {
        if (!_isHeadDetached)
        {
            Debug.Log("BODY DETACH!");
            _isHeadDetached = true;
            DisableMonster();
            StopAlternatingLights();
            head.Detach();
            onDetach.Invoke();
        }
    }

    public void Attach(MonsterHead head)
    {
        if (head == null) throw new System.Exception("No head given!");
        _isHeadDetached = false;
        _isSalted = false;
        _isGarlicked = false;
        this.head = head;
        EnableMonster();
    }
}
