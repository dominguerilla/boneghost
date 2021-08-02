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
    [SerializeField] float cooldownBetweenHits = 1.0f;
    RACE playerRace = RACE.BONE;
    Damageable hitNotifier;

    bool invulnerable = false;

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

        if (detectHits && !invulnerable && projectile.type == ProjectileType.ENEMY)
        {
            StartCoroutine(CooldownHit(projectile));
        }
    }

    IEnumerator CooldownHit(Projectile projectile)
    {
        CustomEvent.Trigger(this.gameObject, "OnProjectileHit", projectile);
        invulnerable = true;
        yield return new WaitForSeconds(cooldownBetweenHits);
        invulnerable = false;
    }

    public RACE GetRace()
    {
        return playerRace;
    }
}
