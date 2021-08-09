using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ProjectileType
{
    PLAYER,
    ENEMY
}

public class Projectile : MonoBehaviour
{
    public ProjectileType type;
    public float flightSpeed = 3f;
    public float damage = 1.0f;
    public float lifetime = 2.0f;
    public int maxNumberOfEntitiesDamaged = 1;

    ProjectilePool pool;
    Vector3 lastLaunchOrigin = Vector3.zero;
    Transform originalParent;

    private void Start()
    {
        originalParent = transform.parent;
    }

    public void SetPool(ProjectilePool pool)
    {
        this.pool = pool;
    }

    public IEnumerator Launch(Vector3 origin, Quaternion orientation, float lifetime)
    {
        transform.parent = null;

        lastLaunchOrigin = origin;
        transform.position = origin;
        transform.rotation = orientation;

        float currentLifetime = 0f;
        while (currentLifetime < lifetime)
        {
            currentLifetime += Time.deltaTime;
            transform.Translate(Vector3.forward * flightSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        ReturnToPool();
    }

    public Vector3 GetLaunchOrigin()
    {
        return lastLaunchOrigin;
    }

    public void ReturnToPool()
    {
        transform.parent = originalParent;
        this.pool.AddToPool(this);
    }
}
