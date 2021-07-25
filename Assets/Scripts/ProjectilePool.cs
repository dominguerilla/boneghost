using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] Projectile[] startingPool;
    Queue<Projectile> availablePool;

    private void Start()
    {
        availablePool = new Queue<Projectile>(startingPool);
        foreach (Projectile projectile in availablePool)
        {
            projectile.gameObject.SetActive(false);
        }
    }

    public void Launch(Vector3 origin, Quaternion orientation, float lifetime)
    {
        Projectile nextProjectile = GetNextProjectile();
        if (nextProjectile) StartCoroutine(Launch(nextProjectile, origin, orientation, lifetime));
        else Debug.LogWarning($"Trying to launch projectile from empty pool { gameObject.name }!");
    }

    Projectile GetNextProjectile() {
        if(availablePool.Count > 0 ) return availablePool.Dequeue();
        return null;
    }

    IEnumerator Launch(Projectile projectile, Vector3 origin, Quaternion orientation, float lifetime)
    {
        Transform originalParent = projectile.transform.parent;
        projectile.transform.parent = null;

        projectile.transform.position = origin;
        projectile.transform.rotation = orientation;
        projectile.gameObject.SetActive(true);

        float currentLifetime = 0f;
        while (currentLifetime < lifetime)
        {
            currentLifetime += Time.deltaTime;
            projectile.transform.Translate(Vector3.forward * projectile.flightSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
        projectile.gameObject.SetActive(false);
        projectile.transform.parent = originalParent;
        availablePool.Enqueue(projectile);
    }
}
