using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO: Consider refactoring this to take a ProjectileShooter instead of a ProjectilePool. Could make it so player could use dropped enemy weapons?
/// </summary>
public class Weapon : ItemComponent
{
    [SerializeField] float attackCooldown = 1.0f;
    [SerializeField] float soundDelay = 0f;
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
        yield return new WaitForSeconds(attackCooldown);
        _isAttacking = false;
        this.onUseEnd.Invoke();
        yield return null;
    }

    void LaunchProjectile()
    {
        projectilePool.Launch(mainCam.transform.position + mainCam.transform.forward, mainCam.transform.rotation, attackCooldown);
    }

    public float GetAttackCooldown()
    {
        return attackCooldown;
    }
}
