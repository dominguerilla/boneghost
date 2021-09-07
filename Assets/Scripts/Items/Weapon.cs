using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// TODO: Consider refactoring this to take a ProjectileShooter instead of a ProjectilePool. Could make it so player could use dropped enemy weapons?
/// </summary>
public class Weapon : ItemComponent
{
    public UnityEvent onUpgrade = new UnityEvent();
    public UnityEvent onStatusApplied = new UnityEvent();

    [SerializeField] float soundDelay = 0f;
   
    [Header("Projectile Settings")]
    [SerializeField] float baseAttackCooldown = 1.0f;
    [SerializeField] float baseAttackRange = 1.0f;
    [SerializeField] Vector3 projectileSpawnOffset;
    [SerializeField] ProjectilePool projectilePool;
    
    
    bool _isAttacking = false;
    Camera mainCam;
    float cooldownFactor = 1.0f;
    Color weaponColor = Color.white;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public override void Use()
    {
        if(!_isAttacking) StartCoroutine(Attack());
    }

    public float GetAttackCooldown()
    {
        return baseAttackCooldown * cooldownFactor;
    }
    public void ApplyStatus(Status status)
    {
        this.weaponColor = status.raceStatus.weaponColor;
        this.cooldownFactor = CalculateCooldown(status);
        projectilePool.ApplyStats(status);
        onStatusApplied.Invoke();
    }

    public Color GetWeaponColor()
    {
        return this.weaponColor;
    }

    IEnumerator Attack()
    {
        _isAttacking = true;
        this.onUseStart.Invoke();
        equippedArm.TriggerAnimation("slash");
        yield return new WaitForSeconds(soundDelay);
        LaunchProjectile();
        PlayOnUseSound(soundDelay);
        yield return new WaitForSeconds(baseAttackCooldown * cooldownFactor);
        _isAttacking = false;
        this.onUseEnd.Invoke();
        yield return null;
    }

    void LaunchProjectile()
    {
        Vector3 launchPosition = mainCam.transform.position + projectileSpawnOffset;
        projectilePool.SetMaxDistance(baseAttackRange);
        projectilePool.Launch(launchPosition + mainCam.transform.forward, mainCam.transform.rotation);
    }

    float CalculateCooldown(Status status)
    {
        float DEX = status.raceStatus.DEX;
        return 1 / DEX;
    }



}
