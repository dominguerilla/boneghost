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
    public bool isInitialized { get; private set; }

    [Header("Player Settings")]
    [SerializeField] bool detectHits = true;
    [SerializeField] float cooldownBetweenHits = 1.0f;
    [SerializeField] InventoryComponent inventory;

    [SerializeField] CLASS currentClass;
    [SerializeField] RACE currentRace;
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
        isInitialized = true;
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

    public void SetClass(CLASS newClass){
        if (newClass != currentClass)
        {
            currentClass = newClass;
            CustomEvent.Trigger(this.gameObject, "OnClassChange", currentClass);
        }
    }

    /// <summary>
    /// Change player race, notifying Bolt. Only triggers if newClass is different from the current class.
    /// </summary>
    public void SetRace(RACE newRace)
    {
        if ( newRace != currentRace)
        {
            currentRace = newRace;
            ApplyRaceChange();
        }
    }

    public void SetRace(string newRace)
    {
        RACE race;
        switch (newRace)
        {
            case "BONE":
                race = RACE.BONE;
                break;
            case "GHOST":
                race = RACE.GHOST;
                break;
            case "DEMON":
                race = RACE.DEMON;
                break;
            default:
                Debug.LogError($"RACE {newRace} NOT FOUND!");
                race = RACE.BONE;
                break;
        }
        SetRace(race);
    }

    public void ApplyRaceChange()
    {
        CustomEvent.Trigger(this.gameObject, "OnRaceChange", currentRace);
        ChangeWeaponColor(currentRace);
    }

    public CLASS GetClass()
    {
        return currentClass;
    }

    public RACE GetRace()
    {
        return currentRace;
    }

    public void InvokeDeath()
    {
        onDeath.Invoke();
    }

    void ChangeWeaponColor(RACE race)
    {
        Color classColor;
        switch (race)
        {
            case RACE.BONE:
                classColor = Color.blue;
                break;
            case RACE.GHOST:
                classColor = Color.green;
                break;
            case RACE.DEMON:
                classColor = Color.red;
                break;
            default:
                classColor = Color.white;
                break;
        }
        inventory.ApplyWeaponColor(classColor);
    }
}
