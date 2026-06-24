using UnityEngine;
using DG.Tweening;

public class TaskTriggerLight : MonoBehaviour
{
    Light _light;
    public float MaxIntensity = 4;
    public float Frequency = 0.3f;

    public void Initialize()
    {
        _light = gameObject.GetComponent<Light>();
    }
    public void StartIndicator()
    {
        _light.DOIntensity(MaxIntensity, Frequency).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    } 

    public void StopIndicator()
    {
        _light.DOKill();
        _light.DOIntensity(0, 0.5f);
    }
}