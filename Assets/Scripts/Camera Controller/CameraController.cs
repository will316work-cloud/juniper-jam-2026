using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float CameraShakeStrength = 1;

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
    }
}