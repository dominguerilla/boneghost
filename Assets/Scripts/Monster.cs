using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using Bolt;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Monster: MonoBehaviour
{
    #region PARAMETERS
    [Header("Monster Parameters")]
    [SerializeField] bool monsterEnabled;
    [SerializeField] float startingHealth = 1.0f;
    //public GameObject[] huntingGrounds;
    //public float huntingGroundRadius = 5f;
    public float stunTime = 2.5f;
    public float attackRange = 10f;
    [SerializeField] Vision vision;

    [Header("Events")]
    public UnityEvent OnDetectPlayer = new UnityEvent();
    public UnityEvent OnLosePlayer = new UnityEvent();
    public UnityEvent OnDestruct = new UnityEvent();
    public UnityEvent OnDeath = new UnityEvent();
    #endregion

    #region PROTECTED MEMBERS
    protected float currentHealth;
    protected bool alive;
    protected NavMeshAgent agent;
    protected bool isStunned = false;
    protected bool isAlternatingLight = false;
    protected GameObject currentTarget = null;
    protected Vector3 lastSeenPlayerPosition = Vector3.zero;
    protected bool _agentInCooldown = false;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Damageable damageable;
    [SerializeField] protected ProjectilePool projectilePool;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip attackSound;


    protected virtual void Awake()
    {
        currentHealth = startingHealth;
        alive = currentHealth > 0f;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if (damageable) damageable.onProjectileHit.AddListener(OnProjectileHit);
        else Debug.LogWarning($"No Damageable set on {gameObject.name}!");
        agent.enabled = monsterEnabled;
    }

    protected IEnumerator StunRoutine()
    {
        isStunned = true;
        bool originalState = agent.isStopped;
        agent.isStopped = true;
        yield return new WaitForSeconds(stunTime);
        agent.isStopped = originalState;
        isStunned = false;
    }
    protected void OnDestroy()
    {
        DisableMonster();
    }

    protected void OnProjectileHit(Projectile projectile)
    {
        if(projectile.type == ProjectileType.PLAYER) TakeDamage(projectile.damage);
    }

    protected void LateUpdate()
    {
        SetAnimBool("isWalking", agent.velocity.magnitude > 0);
    }
    #endregion

    #region PUBLIC MEMBERS
    public void GoTo(Vector3 position)
    {
        // Bolt can call this function before Awake has even run
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        agent.SetDestination(position);
    }

    public bool isAtDestination()
    {
        if (monsterEnabled) {
            return agent.remainingDistance < 0.5f && !agent.pathPending;
        }
        return true;
    }

    public bool isWithinAttackRange(GameObject other)
    {
        if (other == null) return false;
        return Vector3.Distance(this.transform.position, other.transform.position) <= attackRange;
    }

    public bool GetRandomPointInVicinity(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    public virtual void SelfDestruct(float timeToDestroy)
    {
        Debug.Log($"Destroying the monster {this.name}");
        OnDestruct.Invoke();
        Destroy(this.gameObject, timeToDestroy);
    }

    public virtual void OnSalted()
    {
        if (!monsterEnabled) return;
        if(!isStunned) StartCoroutine(StunRoutine());
    }

    public void Stop()
    {
        this.agent.ResetPath();
    }

    public void LookAt(Vector3 position)
    {
        if (!monsterEnabled || isStunned) return;
        this.transform.LookAt(position);
    }

    public virtual void OnGarlicked()
    {
        if (!monsterEnabled) return;
        Debug.Log($"{this.gameObject.name} has been garlicked!");
    }

    public void TriggerOnDetectPlayer()
    {
        if (!monsterEnabled) return;
        currentTarget = vision.GetSeenTarget();
        OnDetectPlayer.Invoke();
    }

    public void TriggerOnLosePlayer()
    {
        if (!monsterEnabled) return;
        lastSeenPlayerPosition = currentTarget.transform.position;
        currentTarget = null;
        OnLosePlayer.Invoke();
    }

    public void EnableMonster()
    {
        monsterEnabled = true;
        agent.enabled = true;
    }

    public void DisableMonster()
    {
        monsterEnabled = false;
        agent.enabled = false;
    }

    public Vector3 GetLastSeenPlayerPosition()
    {
        if (currentTarget)
        {
            return currentTarget.transform.position;
        }
        return lastSeenPlayerPosition;
    }

    public GameObject GetCurrentTargetPlayer()
    {
        return currentTarget;
    }

    public virtual void Attack()
    {
        anim.SetTrigger("Attack");
        if(audioSource) audioSource.PlayOneShot(attackSound);
        projectilePool.Launch((transform.position + new Vector3(0, 0.2f, 0)) + transform.forward, transform.rotation);
    }

    public bool isEnabled()
    {
        return monsterEnabled;
    }

    public virtual void Die()
    {
        anim.SetTrigger("Die");
        alive = false;
        OnDeath.Invoke();
    }

    public virtual void Rise()
    {
        alive = true;
        anim.SetTrigger("Rise");
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public bool isAlive()
    {
        return alive;
    }

    public void SetAnimBool(string name, bool value)
    {
        anim.SetBool(name, value);
    }

    public void SetAnimTrigger(string name)
    {
        anim.SetTrigger(name);
    }

    public void ResetAnimTrigger(string name)
    {
        anim.ResetTrigger(name);
    }
    #endregion
}
