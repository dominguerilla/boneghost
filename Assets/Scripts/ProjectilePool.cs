using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] Projectile[] startingPool;
    Queue<Projectile> availablePool;

    private void Start()
    {
        availablePool = new Queue<Projectile>();
        foreach (Projectile projectile in startingPool)
        {
            AddToPool(projectile);
        }
    }

    public void Launch(Vector3 origin, Quaternion orientation, float lifetime)
    {
        Projectile nextProjectile = GetNextProjectile();
        if (nextProjectile) StartCoroutine(Launch(nextProjectile, origin, orientation, lifetime));
        else Debug.LogWarning($"Trying to launch projectile from empty pool { gameObject.name }!");
    }

    public void Launch(Vector3 origin, Quaternion orientation)
    {
        Projectile nextProjectile = GetNextProjectile();
        if (nextProjectile) StartCoroutine(Launch(nextProjectile, origin, orientation, nextProjectile.lifetime));
        else Debug.LogWarning($"Trying to launch projectile from empty pool { gameObject.name }!");
    }

    public void AddToPool(Projectile projectile)
    {
        projectile.SetPool(this);
        projectile.gameObject.SetActive(false);
        availablePool.Enqueue(projectile);
    }

    Projectile GetNextProjectile() {
        if(availablePool.Count > 0 ) return availablePool.Dequeue();
        return null;
    }

    IEnumerator Launch(Projectile projectile, Vector3 origin, Quaternion orientation, float lifetime)
    {
        projectile.gameObject.SetActive(true);
        yield return projectile.Launch(origin, orientation, lifetime);
    }
}
