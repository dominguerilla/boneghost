using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] Projectile[] startingPool;
    Queue<Projectile> availablePool;

    float maxDistance = 10f;

    private void Start()
    {
        availablePool = new Queue<Projectile>();
        foreach (Projectile projectile in startingPool)
        {
            AddToPool(projectile);
        }
    }

    public void Launch(Vector3 origin, Quaternion rotation)
    {
        Projectile nextProjectile = GetNextProjectile();
        if (nextProjectile) StartCoroutine(Launch(nextProjectile, origin, rotation, nextProjectile.lifetime, maxDistance));
        else Debug.LogWarning($"Trying to launch projectile from empty pool { gameObject.name }!");
    }

    public void AddToPool(Projectile projectile)
    {
        projectile.SetPool(this);
        projectile.gameObject.SetActive(false);
        availablePool.Enqueue(projectile);
    }

    public void SetMaxDistance(float value)
    {
        this.maxDistance = value;
    }

    public void ApplyColor(Color color)
    {
        foreach (Projectile proj in availablePool)
        {
            Renderer projRenderer = proj.GetComponentInChildren<MeshRenderer>();
            projRenderer.material.SetColor("_Color", color);
        }
    }

    Projectile GetNextProjectile() {
        if(availablePool.Count > 0 ) return availablePool.Dequeue();
        return null;
    }

    IEnumerator Launch(Projectile projectile, Vector3 origin, Quaternion orientation, float lifetime, float maxDistance)
    {
        projectile.gameObject.SetActive(true);
        yield return projectile.Launch(origin, orientation, lifetime, maxDistance);
    }
}
