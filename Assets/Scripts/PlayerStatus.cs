using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Mango.Actions;
using UnityEngine.Events;

public enum RACE
{
    BONE,
    GHOST,
    DEMON
}

public enum CLASS
{
    SAMURAI,
    NINJA,
    MONK
}

[RequireComponent(typeof(Damageable))]
public class PlayerStatus : MonoBehaviour
{
    [SerializeField] bool detectHits = true;
    [SerializeField] float cooldownBetweenHits = 1.0f;
    public UnityEvent onDamageTaken = new UnityEvent();
    public UnityEvent onDeath = new UnityEvent();

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
            onDamageTaken.Invoke();
        }
    }

    IEnumerator CooldownHit(Projectile projectile)
    {
        CustomEvent.Trigger(this.gameObject, "OnProjectileHit", projectile);
        invulnerable = true;
        yield return new WaitForSeconds(cooldownBetweenHits);
        invulnerable = false;
    }

    public void SetInvulnerable(bool value)
    {   
        invulnerable = value;
    }

    public void InvokeDeath()
    {
        onDeath.Invoke();
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
