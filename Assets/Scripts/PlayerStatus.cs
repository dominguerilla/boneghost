using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Mango.Actions;

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

    public void SetInvulnerable(bool value)
    {   
        invulnerable = value;
    }

    public void UpgradeWeapon(int armIndex)
    {
        ArmFighter armFighter = GetComponent<ArmFighter>();
        if (armFighter)
        {
            ItemComponent item = armFighter.GetItem(armIndex);
            if (item && item is Weapon)
            {
                Weapon weapon = (Weapon)item;
                float originalAttackCooldown = weapon.GetAttackCooldown();
                weapon.SetAttackCooldown(originalAttackCooldown * 0.5f);
                weapon.onUpgrade.Invoke();
                //Debug.Log($"{weapon.gameObject.name} weapon upgraded!");
            }
        }
    }
}
