using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public int maxNumberOfEntitiesDamaged = 1;

    ProjectilePool pool;

    public void SetPool(ProjectilePool pool)
    {
        this.pool = pool;
    }

    public IEnumerator Launch(Vector3 origin, Quaternion orientation, float lifetime)
    {
        transform.position = origin;
        transform.rotation = orientation;

        float currentLifetime = 0f;
        while (currentLifetime < lifetime)
        {
            currentLifetime += Time.deltaTime;
            transform.Translate(Vector3.forward * flightSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
    }
}
