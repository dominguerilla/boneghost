using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] GameObject UI;
    [SerializeField] Image staminaMeter;

    [Header("UI Settings")]
    [SerializeField] Vector2 anchorMin;
    [SerializeField] Vector2 anchorMax;
    [SerializeField] Vector3 UIposition;

    private void Start()
    {
        weapon.onEquip.AddListener(EnableUI);
        weapon.onDequip.AddListener(DisableUI);
        weapon.onUseStart.AddListener(StartStaminaRefill);
        staminaMeter.rectTransform.anchorMin = anchorMin;
        staminaMeter.rectTransform.anchorMax = anchorMax;
        staminaMeter.rectTransform.anchoredPosition = UIposition;
    }

    void EnableUI()
    {
        UI.SetActive(true);
    }

    void DisableUI()
    {
        UI.SetActive(false);
    }

    void StartStaminaRefill()
    {
        StartCoroutine(ResetAndRefillStamina(weapon.GetAttackCooldown()));
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
