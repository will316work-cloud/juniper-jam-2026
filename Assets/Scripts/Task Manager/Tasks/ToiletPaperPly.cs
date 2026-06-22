using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class ToiletPaperPly : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform PaperPly;
    public float PlySpeed = 0.1f;

    private bool _isDragging;
    private Vector3 _currentMousePosition;
    private Vector3 _previousMousePosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
        _previousMousePosition = Mouse.current.position.ReadValue();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
    }

    void Update()
    {
        if (!_isDragging) return;

        _currentMousePosition = Mouse.current.position.ReadValue();

        float delta = _previousMousePosition.y - _currentMousePosition.y;

        if (delta > 0)
        {
            PaperPly.localPosition -= new Vector3(0, delta * PlySpeed, 0);
        }

        _previousMousePosition = _currentMousePosition;
    }
}