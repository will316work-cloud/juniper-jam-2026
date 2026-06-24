using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PrinterCrank : TaskProp, IPointerDownHandler, IPointerUpHandler
{
    
    public GameObject PivotObj;
    public GameObject PaperObj;
    public Transform PaperStartPosition;
    public Transform PaperEndPosition;
    public float MaxAngleChangeTreshold = 90;
    public int RoundsNeeded;

    private bool _isDragging;
    private bool _taskCompleted;
    private float _previousAngle;
    private float _amountRotated;

    public override void OnTaskStart()
    {
        PaperObj.transform.localPosition = PaperStartPosition.localPosition;
        PivotObj.transform.rotation = Quaternion.Euler(0, 0, 0);
        _amountRotated = 0;
        _taskCompleted = false;
        _isDragging = false;
    }

    public override void PointerDownHandler()
    {
        _isDragging = true;
        _previousAngle = GetMouseAngle();
    }

    public override void PointerUpHandler()
    {
        _isDragging = false;
    }

    void Update()
    {
        if (_isDragging && !_taskCompleted)
        {
            float currentAngle = GetMouseAngle();
            float delta = Mathf.DeltaAngle(_previousAngle, currentAngle);

            if (Mathf.Abs(delta) >= MaxAngleChangeTreshold)
                return;

            if (delta > 0)
            {
                PivotObj.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
                _amountRotated += Mathf.Abs(delta);

                float progress = Mathf.Clamp01(_amountRotated / (RoundsNeeded * 360));
                PaperObj.transform.localPosition = Vector3.Lerp(PaperStartPosition.localPosition, PaperEndPosition.localPosition, progress);

                if (_amountRotated >= RoundsNeeded * 360 && !_taskCompleted)
                {
                    _taskCompleted = true;

                    _ctx.TaskManager.CurrentTask.OnTaskEnd(true);
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