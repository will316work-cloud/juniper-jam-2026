using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;
using UnityEngine.Rendering.Universal;


public class CameraController : MonoBehaviour
{   
    [Header("Camera Shake")]
    public float CameraShakeStrength = 1;
    public float CameraShakeDuration = 0.5f;

    [Header("Flash Indicator")]
    public Image FlashIndicatorImage;
    public Color FlashIndicatorColor;
    public float FlashIndicatorFadeInDuration = 0.05f;
    public float FlashIndicatorFadeOutDuration = 0.1f;


    [Header("Chromatic Aberration")]
    public float ChromaticAberrationIntensity = 1.0f;
    public float ChromaticAberrationFadeInDuration = 0.05f;
    public float ChromaticAberrationFadeOutDuration = 0.1f;
    ChromaticAberration _chromaticAberration;
    Tween _chromaticAberrationSequence;

    [Header("Bloom")]
    public float BloomIntensity = 6f;
    public float BloomTreshold = 1.0f;
    public float BloomFadeInDuration = 0.05f;
    public float BloomFadeOutDuration = 0.1f;
    Bloom _bloom;
    Tween _bloomSequence;

    [Header("Depth of field")]
    public float DepthOfFieldFadeInDuration = 1f;
    DepthOfField _depthOfField;
    Tween _depthOfFieldSequence;

    [Header("Depth of field")]
    CinemachineBasicMultiChannelPerlin _perlin;


    public List<ActionTypeColor> ActionTypeColors = new();

    private CinemachineVolumeSettings _volumeSettings;
    [SerializeField] private CinemachineCamera _menuCamera;
    [SerializeField] private CinemachineCamera _gameCamera;
    [SerializeField] private CinemachineCamera _creditsCamera;
    
    CinemachineCamera _currentCamera;
    CinemachineCamera _previousCamera;
    CinemachineBrain _cameraBrain;

    Dictionary<CameraType, CinemachineCamera> _cameras = new();

    public void Initialize()
    {
        _cameraBrain = Camera.main.GetComponent<CinemachineBrain>();

        _volumeSettings = _gameCamera.GetComponent<CinemachineVolumeSettings>();
        _volumeSettings.Profile.TryGet(out _chromaticAberration);
        _volumeSettings.Profile.TryGet(out _bloom);
        _volumeSettings.Profile.TryGet(out _depthOfField);
        _perlin = _menuCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        // _cameras.Add(CameraType.Menu, _menuCamera);
        _cameras.Add(CameraType.Gameplay, _gameCamera);
        _cameras.Add(CameraType.Menu, _menuCamera);
        _cameras.Add(CameraType.Credits, _creditsCamera);

        _currentCamera = _menuCamera;
    }

    public void SwitchToCamera(CameraType requestedCameraType)
    {
        if (_currentCamera == _cameras[requestedCameraType]) return;

        if(requestedCameraType == CameraType.Menu && _currentCamera != _menuCamera)
        {
            if(_currentCamera != null)
            {
                _previousCamera = _currentCamera;

                if(_previousCamera == _cameras[CameraType.Credits])
                    _cameraBrain.DefaultBlend.Time = 1f;

                _currentCamera.Priority = 0;
            }

            _currentCamera = _cameras[CameraType.Menu];
            _currentCamera.Priority = 10;
            _perlin.enabled = true;
        }
        else if(requestedCameraType == CameraType.Gameplay && _currentCamera != _gameCamera)
        {
            _cameraBrain.DefaultBlend.Time = 0;

            if(_currentCamera != null)
            {
                _currentCamera.Priority = 0;
                _previousCamera = _currentCamera;
            }

            _currentCamera = _cameras[CameraType.Gameplay];
            _currentCamera.Priority = 10;
            _perlin.enabled = false;
        }
        else if(requestedCameraType == CameraType.Credits && _currentCamera != _gameCamera)
        {
            if(_currentCamera != null)
            {
                _previousCamera = _currentCamera;

                if(_previousCamera == _cameras[CameraType.Menu])
                    _cameraBrain.DefaultBlend.Time = 1f;

                _currentCamera.Priority = 0;
                _previousCamera = _currentCamera;
            }

            _currentCamera = _cameras[CameraType.Credits];
            _currentCamera.Priority = 10;
            _perlin.enabled = false;
        }
    }

    public Color GetColor(EffectType type)
    {
        foreach (ActionTypeColor actionTypeColor in ActionTypeColors)
        {
            if (actionTypeColor.ActionType == type)
            {
                return actionTypeColor.Color;
            }
        }

        return Color.white;
    }

    #region Effects
    public void ShakeCamera(EffectType type = EffectType.TaskSuccess)
    {
        _currentCamera.transform.DOKill();
        _currentCamera.transform.DOShakePosition(CameraShakeDuration, CameraShakeStrength);
        FlashScreen(type);
    }

    public void FlashScreen(EffectType type = EffectType.TaskSuccess)
    {
        FlashIndicatorImage.DOKill();
        FlashIndicatorImage.color = GetColor(type);
        ApplyChromaticAberration();
        FlashIndicatorImage.DOFade(1, FlashIndicatorFadeInDuration).OnComplete(() => FlashIndicatorImage.DOFade(0, FlashIndicatorFadeOutDuration));
    }

    public void ApplyChromaticAberration()
    {    
        DOTween.Kill(_chromaticAberrationSequence);

        _chromaticAberrationSequence = DOTween.To(() => _chromaticAberration.intensity.value, x => _chromaticAberration.intensity.value = x, ChromaticAberrationIntensity, ChromaticAberrationFadeInDuration)
            .OnComplete(() => DOTween.To(() => _chromaticAberration.intensity.value, x => _chromaticAberration.intensity.value = x, 0, ChromaticAberrationFadeOutDuration));
    }

    public void ApplyBloom(EffectType type = EffectType.TaskSuccess)
    {
        DOTween.Kill(_bloomSequence);

        _bloom.tint.value = GetColor(type);
        _bloomSequence = DOTween.To(() => _bloom.intensity.value, x => _bloom.intensity.value = x, BloomIntensity, BloomFadeInDuration)
            .OnComplete(() => DOTween.To(() => _bloom.intensity.value, x => _bloom.intensity.value = x, 0, BloomFadeOutDuration));
    }

    public void SetIsDepthOfFieldEnabled(bool isEnabled)
    {
        DOTween.Kill(_depthOfFieldSequence);

        if(isEnabled)
            _depthOfFieldSequence = DOTween.To(() => _depthOfField.focusDistance.value, x => _depthOfField.focusDistance.value = x, 1, DepthOfFieldFadeInDuration).SetEase(Ease.Linear);
        
        else
            _depthOfFieldSequence = DOTween.To(() => _depthOfField.focusDistance.value, x => _depthOfField.focusDistance.value = x, 30, DepthOfFieldFadeInDuration).SetEase(Ease.Linear);
    }
    #endregion
}

[System.Serializable]
public class ActionTypeColor
{
    public EffectType ActionType;
    public Color Color;
}