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
    public float damage = 1.0f;
    public float flightSpeed = 3f;
    public float lifetime = 2.0f;
    public int maxNumberOfEntitiesDamaged = 1;

    ProjectilePool pool;
    Vector3 lastLaunchOrigin = Vector3.zero;
    Vector3 lastTargetLocation = Vector3.zero;
    Transform originalParent;

    bool _isLaunching = false;
    
    private void Start()
    {
        originalParent = transform.parent;
    }

    protected Vector3 GetTargetLocation(Vector3 projectileOrigin, Vector3 direction, float maxDistance)
    {
        RaycastHit hit;
        Ray ray = new Ray(projectileOrigin, direction);
        if (Physics.Raycast(ray, out hit, maxDistance, ~LayerMask.GetMask("Player", "Enemy", "UI", "Projectile")))
        {
            Debug.Log($"{gameObject.name} targeting raycast hit {hit.collider.name} : {hit.point}");
            return hit.point;
        }
        else
        {
            Debug.Log($"{gameObject.name} targeting ray: {ray.GetPoint(maxDistance)}");
            return ray.GetPoint(maxDistance);
        }
    }

    public void SetPool(ProjectilePool pool)
    {
        this.pool = pool;
    }

    private void OnDrawGizmos()
    {
        if (_isLaunching)
        {
            Gizmos.DrawLine(lastLaunchOrigin, lastTargetLocation);
        }
    }

    public IEnumerator Launch(Vector3 origin, Quaternion orientation, float lifetime, float maxDistance)
    {
        _isLaunching = true;
        transform.parent = null;
        transform.position = origin;
        transform.rotation = orientation;

        lastLaunchOrigin = origin;
        lastTargetLocation = GetTargetLocation(origin, transform.forward, maxDistance);

        float distanceToTarget = Vector3.Distance(origin, lastTargetLocation);

        float currentLifetime = 0f;
        while (currentLifetime < lifetime && Vector3.Distance(origin, transform.position) < distanceToTarget)
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
        _isLaunching = false;
        transform.parent = originalParent;
        this.pool.AddToPool(this);
    }
}
