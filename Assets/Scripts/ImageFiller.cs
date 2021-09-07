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
    Color originalColor;

    private void Start()
    {
        if (targetImage)
        {
            originalColor = targetImage.color;
        }
    }
    public void ResetAndFillImage(float fillTime)
    {
        if(targetImage.isActiveAndEnabled) fillRoutine = StartCoroutine(ResetAndFill(fillTime));
    }

    IEnumerator ResetAndFill(float fillTime)
    {
        float currentFillAmount = 0;
        targetImage.fillAmount = currentFillAmount;
        targetImage.color = Color.gray;
        while (targetImage.fillAmount < 1.0f)
        {
            currentFillAmount += Time.deltaTime;
            targetImage.fillAmount = currentFillAmount / fillTime;
            yield return new WaitForEndOfFrame();
        }
        ResetImage();
    }

    void ResetImage()
    {
        if (targetImage)
        {
            targetImage.color = originalColor;
            targetImage.fillAmount = 1.0f;
        }
    }

    private void OnDisable()
    {
        ResetImage();
    }
}
