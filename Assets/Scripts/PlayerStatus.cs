using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public enum RACE
{
    BONE,
    GHOST
}

[RequireComponent(typeof(Damageable))]
public class PlayerStatus : MonoBehaviour
{
    [SerializeField] bool detectHits = true;
    RACE playerRace = RACE.BONE;
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

        if(detectHits && projectile.type == ProjectileType.ENEMY) CustomEvent.Trigger(this.gameObject, "OnProjectileHit", projectile);
    }

    public RACE GetRace()
    {
        return playerRace;
    }
}
