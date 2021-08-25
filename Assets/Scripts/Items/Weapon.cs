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

    [SerializeField] float soundDelay = 0f;
   
    [Header("Projectile Settings")]
    [SerializeField] float baseAttackCooldown = 1.0f;
    [SerializeField] float baseAttackRange = 1.0f;
    [SerializeField] Vector3 projectileSpawnOffset;
    [SerializeField] ProjectilePool projectilePool;
    
    
    bool _isAttacking = false;
    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public override void Use()
    {
        if(!_isAttacking) StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        _isAttacking = true;
        this.onUseStart.Invoke();
        equippedArm.TriggerAnimation("slash");
        yield return new WaitForSeconds(soundDelay);
        LaunchProjectile();
        PlayOnUseSound(soundDelay);
        yield return new WaitForSeconds(baseAttackCooldown);
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

    public float GetAttackCooldown()
    {
        return baseAttackCooldown;
    }

    public void SetAttackCooldown(float value)
    {
        baseAttackCooldown = value;
    }

    public void ApplyStatus(Status status)
    {
        projectilePool.ApplyStats(status);
    }
}
