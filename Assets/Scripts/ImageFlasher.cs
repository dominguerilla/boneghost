using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFlasher : MonoBehaviour
{
    public float imageFadeSpeed = 1f;
    public Image damageImage;

    public void FlashDamageImage()
    {
        StartCoroutine(FlashImage(damageImage, Color.red));
    }

    IEnumerator FlashImage(Image image, Color imageColor)
    {
        Color originalColor = imageColor;
        float alpha = 0.75f;
        Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        image.color = newColor;

        while (image.color.a > 0)
        {
            newColor.a -= Time.deltaTime * imageFadeSpeed;
            image.color = newColor;
            yield return new WaitForEndOfFrame();
        }
    }
}
