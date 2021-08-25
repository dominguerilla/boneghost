using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Mango.Actions;
using UnityEngine.Events;


[RequireComponent(typeof(Damageable))]
public class PlayerStatus : MonoBehaviour
{
    public bool isInitialized { get; private set; }

    [Header("Player Settings")]
    [SerializeField] bool detectHits = true;
    [SerializeField] float cooldownBetweenHits = 1.0f;
    [SerializeField] InventoryComponent inventory;

    public UnityEvent onDamageTaken = new UnityEvent();
    public UnityEvent onRaceChange = new UnityEvent();
    public UnityEvent onDeath = new UnityEvent();

    Damageable hitNotifier;
    bool invulnerable = false;
    Status currentStatus = new Status(RaceConfig.MORTAL, CLASS.NONE);

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
        if (newClass != currentStatus.jobClass)
        {
            currentStatus.jobClass = newClass;
            CustomEvent.Trigger(this.gameObject, "OnClassChange", currentStatus.jobClass);
        }
    }

    /// <summary>
    /// Change player race, notifying Bolt. Only triggers if newClass is different from the current class.
    /// </summary>
    public void SetRace(RaceStatus newRace)
    {
        if ( newRace.race != currentStatus.raceStatus.race)
        {
            currentStatus.raceStatus = newRace;
            ApplyRaceChange();
        }
    }

    public void SetRace(string newRace)
    {
        RaceStatus race;
        switch (newRace)
        {
            case "BONE":
                race = RaceConfig.BONE;
                break;
            case "GHOST":
                race = RaceConfig.GHOST;
                break;
            case "DEMON":
                race = RaceConfig.DEMON;
                break;
            case "VOID":
                race = RaceConfig.VOID;
                break;
            default:
                Debug.LogError($"RACE {newRace} NOT FOUND!");
                race = RaceConfig.MORTAL;
                break;
        }
        SetRace(race);
    }

    public void ApplyRaceChange()
    {
        CustomEvent.Trigger(this.gameObject, "OnRaceChange", currentStatus.raceStatus.race);
        ChangeWeaponColor(currentStatus);
        onRaceChange.Invoke();
    }

    public CLASS GetClass()
    {
        return currentStatus.jobClass;
    }

    public RaceStatus GetRace()
    {
        return currentStatus.raceStatus;
    }

    public void InvokeDeath()
    {
        onDeath.Invoke();
    }

    void ChangeWeaponColor(Status status)
    {

        inventory.ApplyWeaponColor(status.raceStatus.weaponColor);
    }
}
