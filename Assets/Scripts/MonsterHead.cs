using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterHead : Monster
{
    [SerializeField] MonsterLowerHalf attachedBody;
    public float attackCooldown = 2.0f;
    public UnityEvent onDetach = new UnityEvent();
    public UnityEvent onAttach = new UnityEvent();

    Vector3 originalPosition;
    Quaternion originalRotation;

    bool _isAttached = false;
    bool _isAttackOnCooldown = false;

    protected override void Awake()
    {
        base.Awake();
        _isAttached = attachedBody != null;
    }

    public void Attack(Vector3 direction)
    {
        if (!_isAttackOnCooldown)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, attackRange))
            {
                if (hit.transform.tag == "Player")
                {
                    Player player = hit.transform.GetComponent<Player>();
                    //player.ChangeHealth(-1);
                }
            }
            StartCoroutine(AttackCooldown());
        }
    }

    public Vector3 GetDirectionOfPlayer()
    {
        return GetLastSeenPlayerPosition() - transform.position;
    }

    IEnumerator AttackCooldown()
    {
        _isAttackOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        _isAttackOnCooldown = false;
    }

    public bool isAttached()
    {
        return _isAttached;
    }

    public void Detach()
    {
        if (!_isAttached) throw new System.Exception("Head already detached!");
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
        Debug.Log("HEAD DETACH!");
        _isAttached = false;
        this.transform.parent = null;
        EnableMonster();
        onDetach.Invoke();
    }

    public void Attach(MonsterLowerHalf lowerHalf)
    {
        if (_isAttached) throw new System.Exception("Head already attached!");
        if (lowerHalf == null)
        {
            Debug.Log("No lower half given!");
            return;
        }
        //StopAlternatingLights();
        attachedBody = lowerHalf;
        _isAttached = true;
        this.transform.parent = attachedBody.transform;
        this.transform.position = originalPosition;
        this.transform.rotation = originalRotation;
        DisableMonster();
        lowerHalf.Attach(this);
        onAttach.Invoke();
    }

    public MonsterLowerHalf GetBody()
    {
        return attachedBody;
    }

    public void ReAttach()
    {
        Attach(attachedBody);
    }
}
