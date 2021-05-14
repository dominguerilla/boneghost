using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    public Image image;

    public void Awake()
    {
        if (!image) image = GetComponent<Image>();
    }

    public void FadeIn(float duration)
    {
        StartCoroutine(FadeImageIn(duration));
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(FadeImageOut(duration));
    }

    public void Flash(float duration)
    {
        StartCoroutine(FlashImage(duration));
    }

    IEnumerator FlashImage(float duration)
    {
        yield return FadeImageIn(duration / 2);
        yield return FadeImageOut(duration / 2);
    }

    IEnumerator FadeImageIn(float duration)
    {
        float elapsedTime = 0.0f;
        Color c = image.color;
        while (elapsedTime < duration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / duration);
            image.color = c;
        }
    }

    IEnumerator FadeImageOut(float duration)
    {
        float elapsedTime = duration;
        Color c = image.color;
        while (elapsedTime > 0)
        {
            yield return null;
            elapsedTime -= Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / duration);
            image.color = c;
        }
    }
}
