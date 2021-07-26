using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class PlayerStatus : MonoBehaviour
{
    Damageable hitNotifier;

    void Awake()
    {
        hitNotifier = GetComponent<Damageable>();
    }

    private void Start()
    {
        hitNotifier.onProjectileHit.AddListener(OnProjectileHit);
    }

    void OnProjectileHit(Projectile projectile)
    {
        Debug.Log($"{gameObject.name} hit by { projectile.gameObject.name }!");
    }

}
