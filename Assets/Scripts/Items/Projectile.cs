using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    PLAYER,
    ENEMY
}

public class Projectile : MonoBehaviour
{
    public ProjectileType type;
    public float flightSpeed = 3f;
    public float damage = 1.0f;
    public int maxNumberOfEntitiesDamaged = 1;
}
