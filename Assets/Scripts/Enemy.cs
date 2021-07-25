using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] ProjectilePool projectilePool;
    [SerializeField] float timeBetweenProjectiles = 2.0f;
    [SerializeField] float projectileLifetime = 1.0f;

    Rigidbody[] rigidbodies;

    private void Awake()
    {
        if (!projectilePool) projectilePool = GetComponent<ProjectilePool>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(ShootEvery(timeBetweenProjectiles, projectileLifetime));
    }

    IEnumerator ShootEvery(float timeBetweenProjectiles, float projectileLifetime)
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenProjectiles);
            projectilePool.Launch((transform.position + new Vector3(0, 0.2f, 0)) + transform.forward, transform.rotation, projectileLifetime);
        }
    }

    void DisableRigidbodies()
    {
        foreach (Rigidbody body in rigidbodies)
        {
            body.isKinematic = true;
        }
    }
}
