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
    public GameObject[] huntingGrounds;
    public float huntingGroundRadius = 5f;
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
    protected bool isAlive;
    protected NavMeshAgent agent;
    protected bool isStunned = false;
    protected bool isAlternatingLight = false;
    protected GameObject currentTarget = null;
    protected Vector3 lastSeenPlayerPosition = Vector3.zero;
    protected bool _agentInCooldown = false;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Damageable damageable;

    int currentHuntingGroundIndex = 0;

    protected virtual void Awake()
    {
        currentHealth = startingHealth;
        isAlive = currentHealth > 0f;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if (damageable) damageable.onProjectileHit.AddListener(OnProjectileHit);
        else Debug.LogWarning($"No Damageable set on {gameObject.name}!");
        agent.enabled = monsterEnabled;
    }
    protected Vector3 GetRandomHuntingGroundPosition()
    {
        GameObject randomHuntingGround = GetRandomHuntingGround();
        float randX = Random.Range(0.5f, huntingGroundRadius);
        float randZ = Random.Range(0.5f, huntingGroundRadius);

        Vector3 randomOffset = new Vector3(randX, 0, randZ);
        return randomHuntingGround.transform.position + randomOffset;
    }

    protected GameObject GetRandomHuntingGround()
    {
        return huntingGrounds[Random.Range(0, huntingGrounds.Length)];
    }

    protected IEnumerator AlternateLightColors()
    {
        if (!isAlternatingLight)
        {
            isAlternatingLight = true;
            Color lightColor = Color.red;
            while (isAlternatingLight)
            {
                //monsterDebugLight.color = lightColor;
                yield return new WaitForSeconds(0.5f);
                lightColor = lightColor == Color.red ? Color.blue : Color.red;
            }
        }
        yield return null;
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
        TakeDamage(projectile.damage);
    }
    #endregion

    #region PUBLIC MEMBERS
    public void GoToRandomHuntingGround()
    {
        if (huntingGrounds == null || huntingGrounds.Length == 0) return;
        Vector3 huntingGroundPosition = GetRandomHuntingGroundPosition();


        GoTo(huntingGroundPosition);
    }

    public void GoToNextHuntingGround()
    {
        if (huntingGrounds == null || huntingGrounds.Length == 0) return;
        Debug.Log($"{gameObject.name} going to {huntingGrounds[currentHuntingGroundIndex]}");
        GoTo(huntingGrounds[currentHuntingGroundIndex].transform.position);
        currentHuntingGroundIndex = (currentHuntingGroundIndex + 1) % huntingGrounds.Length;
    }

    public void GoTo(Vector3 position)
    {
        // Bolt can call this function before Awake has even run
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        anim.SetBool("isWalking", true);
        agent.SetDestination(position);
    }

    public bool isAtDestination()
    {
        if (monsterEnabled) {
            if (agent.stoppingDistance == 0) return agent.remainingDistance < 1f;
            return agent.remainingDistance < agent.stoppingDistance;
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
        anim.SetBool("isWalking", false);
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
        //monsterDebugLight.color = Color.yellow;
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

    public void StartAlternateLightColors()
    {
        StartCoroutine(AlternateLightColors());
    }

    public Vector3 GetLastSeenPlayerPosition()
    {
        if (currentTarget)
        {
            return currentTarget.transform.position;
        }
        return lastSeenPlayerPosition;
    }

    public void StopAlternatingLights()
    {
        isAlternatingLight = false;
    }

    public GameObject GetCurrentTargetPlayer()
    {
        return currentTarget;
    }

    public virtual void Attack()
    {
        //throw new System.Exception("Not implemented!");
        Debug.Log($"{gameObject.name} attack not implemented!");
    }

    public bool isEnabled()
    {
        return monsterEnabled;
    }

    public virtual void Die()
    {
        anim.SetTrigger("Die");
        isAlive = false;
        OnDeath.Invoke();
    }

    public virtual void Rise()
    {
        isAlive = true;
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

    public bool isNotDead()
    {
        return isAlive;
    }
    #endregion
}
