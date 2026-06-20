using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PrinterCrank : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject PivotObj;
    public int RoundsNeeded;
    bool _isDragging;
    float _previousAngle;
    float _amountRotated;

    GameContext _ctx;

    public void Initialize(GameContext ctx)
    {
        _ctx = ctx;
    }
    public void OnTaskStart()
    {
        PivotObj.transform.rotation = Quaternion.Euler(0, 0, 0);
        _amountRotated = 0;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
        _previousAngle = GetMouseAngle();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
    }

    void Update()
    {
        if (_isDragging)
        {
            float currentAngle = GetMouseAngle();
            float delta = Mathf.DeltaAngle(_previousAngle, currentAngle);

            if (delta > 0)
            {
                PivotObj.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
                _amountRotated += Mathf.Abs(delta);

                if(_amountRotated >= RoundsNeeded * 360)
                {
                    _ctx.TaskManager.CurrentTask.OnTaskSuccess();
                }
            }

            _previousAngle = currentAngle;
        }
    }

    float GetMouseAngle()
    {
        Vector2 pivotScreenPos = PivotObj.transform.position;
        Vector2 direction = (Vector2)Mouse.current.position.ReadValue() - pivotScreenPos;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}