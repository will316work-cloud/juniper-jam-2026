using DG.Tweening;
using TMPro;
using UnityEngine;

public static class UiEffectHandler
{
    public static void BounceTransform(Type bounceType, Transform element, float duration = 0.3f , float strength = 1f)
    {
        float bounceStrength = strength;

        if(bounceType is Type.In)
            bounceStrength *= -1;

        if(bounceType is Type.Shake)
        {
            element.DOShakeScale(duration, bounceStrength);
            return;
        }
            
        else
        {
            float scaleStrength = bounceStrength*0.2f;
            element.DOScale(new Vector3(1 + scaleStrength, 1 + scaleStrength, 1), duration/2)
                .OnComplete(() => element.DOScale(new Vector3(1, 1, 1), duration));
        }   
    }

    public static void BounceText(Type bounceType, TextMeshProUGUI text, float duration = 0.3f , float strength = 3f, EffectColorChangeType colorChangeType = EffectColorChangeType.None)
    {
        Color originalColor = text.color;
        Color colorToFadeTo = colorChangeType is EffectColorChangeType.Red ? Color.red : Color.green;

        if(colorChangeType != EffectColorChangeType.None)
            text.DOColor(colorToFadeTo, duration/2).OnComplete(() => text.DOColor(originalColor, duration));

        if(bounceType is Type.Shake)
        {
            DOTween.Shake(() => text.rectTransform.anchoredPosition, x => text.rectTransform.anchoredPosition = x, duration, strength);
            text.rectTransform.DOShakeRotation(duration, strength).OnComplete(() => text.rectTransform.rotation = Quaternion.Euler(0, 0, 0));;
            return;
        }

        float scaleStrength = strength*0.1f;

        text.rectTransform.DOScale(new Vector3(1 + scaleStrength, 1 + scaleStrength, 1), duration/2)
            .OnComplete(() => text.rectTransform.DOScale(new Vector3(1, 1, 1), duration));

        text.rectTransform.DOShakeRotation(duration, strength).OnComplete(() => text.rectTransform.rotation = Quaternion.Euler(0, 0, 0));
    }
}

public enum Type
{
    Out,
    In,
    Shake
}