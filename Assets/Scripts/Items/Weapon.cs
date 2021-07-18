using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ItemComponent
{
    [SerializeField] float attackCooldown = 1.0f;
    [SerializeField] float soundDelay = 0f;

    Coroutine attackRoutine;
    bool _isAttacking = false;

    public override void Use()
    {
        if(!_isAttacking) attackRoutine = StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        _isAttacking = true;
        this.onUseStart.Invoke();
        equippedArm.TriggerAnimation("slash");
        PlayOnUseSound(soundDelay);
        yield return new WaitForSeconds(attackCooldown);
        _isAttacking = false;
        this.onUseEnd.Invoke();
        yield return null;
    }
}
