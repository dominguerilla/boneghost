using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] GameObject UI;
    [SerializeField] Image staminaMeter;

    private void Start()
    {
        if(weapon) RegisterWeapon(weapon);
    }

    public void RegisterWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        this.weapon.onUseStart.AddListener(StartStaminaRefill);
        this.weapon.onDequip.AddListener(UnregisterWeapon);
        this.weapon.onUpgrade.AddListener(OnUpgrade);
        EnableUI();
    }

    public void UnregisterWeapon()
    {
        DisableUI();
        this.weapon.onUseStart.RemoveListener(StartStaminaRefill);
        this.weapon.onDequip.RemoveListener(UnregisterWeapon);
        this.weapon.onUpgrade.RemoveListener(OnUpgrade);
        this.weapon = null;
    }

    public void EnableUI()
    {
        UI.SetActive(true);
    }

    public void DisableUI()
    {
        UI.SetActive(false);
    }

    void StartStaminaRefill()
    {
        StartCoroutine(ResetAndRefillStamina(weapon.GetAttackCooldown()));
    }

    void OnUpgrade()
    {
        staminaMeter.color = Color.yellow;
    }

    IEnumerator ResetAndRefillStamina(float cooldown)
    {
        float currentStamina = 0;
        Color originalColor = staminaMeter.color;
        staminaMeter.fillAmount = currentStamina;
        staminaMeter.color = Color.gray;
        while (staminaMeter.fillAmount < 1.0f)
        {
            currentStamina += Time.deltaTime;
            staminaMeter.fillAmount = currentStamina / cooldown;
            yield return new WaitForEndOfFrame();
        }
        staminaMeter.color = originalColor;
    }
}
