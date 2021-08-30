using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Use to set the fill amount of an image, as well as gradually increase fill amount over time.
/// </summary>
public class ImageFiller : MonoBehaviour
{
    [SerializeField] Image targetImage;

    Coroutine fillRoutine;
    public void ResetAndFillImage(float fillTime)
    {
        fillRoutine = StartCoroutine(ResetAndFill(fillTime));
    }

    IEnumerator ResetAndFill(float fillTime)
    {
        float currentFillAmount = 0;
        Color originalColor = targetImage.color;
        targetImage.fillAmount = currentFillAmount;
        targetImage.color = Color.gray;
        while (targetImage.fillAmount < 1.0f)
        {
            currentFillAmount += Time.deltaTime;
            targetImage.fillAmount = currentFillAmount / fillTime;
            yield return new WaitForEndOfFrame();
        }
        targetImage.color = originalColor;
    }
}
