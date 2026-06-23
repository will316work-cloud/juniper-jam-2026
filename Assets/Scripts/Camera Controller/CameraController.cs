using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public float CameraShakeStrength = 1;
    public Image FlashIndicatorImage;
    public Color FlashIndicatorColor;

    Camera _menuCamera;
    Camera _gameCamera;
    Camera _currentCamera;

    Dictionary<CameraType, Camera> _cameras = new();

    public void Initialize()
    {
        // _menuCamera = GameObject.FindWithTah("MenuCamera").GetComponent<Camera>();
        _gameCamera = Camera.main;
        _currentCamera = _gameCamera;
    }

    public void SwitchToGameCamera(CameraType cameraType)
    {
        _currentCamera.gameObject.SetActive(false);
        _currentCamera = _cameras[cameraType];
        _currentCamera.gameObject.SetActive(true);
    }

    public void ShakeCamera()
    {
        _currentCamera.DOShakePosition(0.5f, CameraShakeStrength);
        FlashScreen(FlashIndicatorColor);
    }

    public void FlashScreen(Color color)
    {
        FlashIndicatorImage.color = color;
        FlashIndicatorImage.DOFade(1, 0.05f).OnComplete(() => FlashIndicatorImage.DOFade(0, 0.1f));
    }
}