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
        CustomEvent.Trigger(this.gameObject, "OnProjectileHit", projectile);
    }

    public RACE GetRace()
    {
        return playerRace;
    }
}
