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
}
