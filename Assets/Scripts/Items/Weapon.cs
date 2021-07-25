using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ItemComponent
{
    [SerializeField] float attackCooldown = 1.0f;
    [SerializeField] float soundDelay = 0f;
    [SerializeField] Vector3 hitboxHalfExtents;
    [SerializeField] ProjectilePool projectilePool;

    Coroutine attackRoutine;
    bool _isAttacking = false;
    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }


    public override void Use()
    {
        if(!_isAttacking) attackRoutine = StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        _isAttacking = true;
        this.onUseStart.Invoke();
        equippedArm.TriggerAnimation("slash");
        yield return new WaitForSeconds(soundDelay);
        //CheckForHits();
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

    private void OnDrawGizmosSelected()
    {
        if (isEquipped)
        {
            Gizmos.matrix = mainCam.transform.localToWorldMatrix;
            Gizmos.color = _isAttacking ? Color.red : Color.blue;
            Gizmos.DrawCube(Vector3.zero + 2 * Vector3.forward, 2 * hitboxHalfExtents);
        }
    }

    void CheckForHits()
    {
        Collider[] hitObjects = Physics.OverlapBox(mainCam.transform.position + 2 * mainCam.transform.forward, hitboxHalfExtents, Quaternion.identity, LayerMask.GetMask("Default"));
        foreach (Collider col in hitObjects) {
            IDestructible destructible = col.GetComponent<IDestructible>();
            if (destructible != null)
            {
                destructible.OnAttacked(this.gameObject);
            }
        }
    }

    public float GetAttackCooldown()
    {
        return attackCooldown;
    }
}
