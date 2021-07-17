using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ItemComponent
{
    [SerializeField] float attackCooldown = 1.0f;


    Coroutine attackRoutine;
    bool _isAttacking = false;

    public override void Use()
    {
        if(!_isAttacking) attackRoutine = StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        _isAttacking = true;
        equippedArm.TriggerAnimation("slash");
        yield return new WaitForSeconds(attackCooldown);
        _isAttacking = false;
        yield return null;
    }
}
