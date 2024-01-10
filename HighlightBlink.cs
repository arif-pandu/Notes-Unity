using System;
using UnityEngine;

public static class HighlightBlink
{
    public static float blinkDuration = .2f;
    public static float blinkIntensity = .8f;

    public static void BlinkObject(GameObject obj, Action onComplete)
    {
        if (obj != null)
        {
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                float initialLuminosity = spriteRenderer.color.grayscale;

                LeanTween.value(obj, initialLuminosity, initialLuminosity * blinkIntensity, blinkDuration)
                    .setOnUpdate((float value) =>
                    {
                        Color newColor = new(value, value, value, spriteRenderer.color.a);
                        spriteRenderer.color = newColor;
                        
                    })
                    .setEase(LeanTweenType.easeInOutQuad)
                    .setLoopPingPong(1)
                    .setOnComplete(() =>
                    {
                        spriteRenderer.color = new Color(initialLuminosity, initialLuminosity, initialLuminosity, spriteRenderer.color.a);
                        onComplete?.Invoke();
                    });
            }
            else
            {
                Debug.LogWarning("The provided GameObject does not have a SpriteRenderer component.");
            }
        }
        else
        {
            Debug.LogWarning("The provided GameObject is null.");
        }
    }
}
