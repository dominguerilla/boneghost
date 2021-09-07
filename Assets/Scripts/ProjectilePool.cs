using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] Projectile[] startingPool;
    Queue<Projectile> availablePool;

    float maxDistance = 10f;

    private void Awake()
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
        if (nextProjectile) StartCoroutine(Launch(nextProjectile, origin, rotation, maxDistance));
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

    void ApplyColor(Projectile projectile, Color color)
    {
        Renderer projRenderer = projectile.GetComponentInChildren<MeshRenderer>();
        projRenderer.material.SetColor("_Color", color);
    }

    public void ApplyStats(Status status)
    {
        Color color = status.raceStatus.weaponColor;
        foreach (Projectile proj in availablePool)
        {
            ApplyColor(proj, color);
            proj.SetBonusFactors(status.raceStatus.STR, status.raceStatus.DEX, status.raceStatus.INT);
        }
    }

    Projectile GetNextProjectile() {
        if(availablePool.Count > 0 ) return availablePool.Dequeue();
        return null;
    }

    IEnumerator Launch(Projectile projectile, Vector3 origin, Quaternion orientation, float maxDistance)
    {
        projectile.gameObject.SetActive(true);
        yield return projectile.Launch(origin, orientation, maxDistance);
    }
}
