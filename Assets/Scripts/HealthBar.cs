using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

/// <summary>
/// https://feel-docs.moremountains.com/recipes.html
/// A simple class used to interact with a MMHealthBar component and test it
/// To use it, add it to an object with a MMHealthBar, and at runtime, move its CurrentHealth slider, and press the Test button to update the bar
/// </summary>
/// TODO: Generalize this to subscribe to some interface, not just Monster
public class HealthBar : MonoBehaviour
{
    [SerializeField]
    protected Monster entity;

    [Range(0f, 100f)]
    [SerializeField] protected float CurrentHealth = 50f;

    protected float _minimumHealth = 0f;
    protected float _maximumHealth = 100f;
    protected MMHealthBar _targetHealthBar;

    [MMInspectorButton("Test")] public bool TestButton;

    protected virtual void Awake()
    {
        _targetHealthBar = this.gameObject.GetComponent<MMHealthBar>();
    }

    protected virtual void Start()
    {
        SetHealthLimit(0f, entity.GetMaxHealth());
        entity.OnDamage.AddListener(UpdateBar);
    }

    public void SetHealthLimit(float min, float max)
    {
        _minimumHealth = min;
        _maximumHealth = max;
    }

    public void UpdateBar()
    {
        CurrentHealth = entity.GetCurrentHealth();
        _targetHealthBar.UpdateBar(CurrentHealth, _minimumHealth, _maximumHealth, true);
    }

    public virtual void Test()
    {
        if (_targetHealthBar != null)
        {
            _targetHealthBar.UpdateBar(CurrentHealth, _minimumHealth, _maximumHealth, true);
        }
    }
}