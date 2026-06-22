using UnityEngine;
using UnityEngine.InputSystem;

public class CoolerWaterBottle : TaskProp
{
    [Header("Bottle UI")]
    public RectTransform WaterBottle;

    [Tooltip("How much the bottle moves down per mouse pixel moved left")]
    public float ScrewSpeed = 0.1f;

    [Tooltip("How many UI units the bottle must move down to finish")]
    public float RequiredDropDistance = 100f;

    private Vector2 _startPosition;
    private float _droppedAmount;

    private bool _isHolding;
    private bool _taskFinished;

    private Vector2 _previousMouse;

    public override void Initialize(GameContext ctx)
    {
        base.Initialize(ctx);

        if (WaterBottle != null)
            _startPosition = WaterBottle.anchoredPosition;
    }

    public override void OnTaskStart()
    {
        ResetTaskState();
    }

    public override void PointerDownHandler()
    {
        if (_taskFinished)
            return;

        _isHolding = true;

        if (Mouse.current != null)
            _previousMouse = Mouse.current.position.ReadValue();
    }

    public override void PointerUpHandler()
    {
        _isHolding = false;
    }

    private void Update()
    {
        if (!_isHolding || _taskFinished || Mouse.current == null || WaterBottle == null)
            return;

        Vector2 currentMouse = Mouse.current.position.ReadValue();

        float deltaLeft = _previousMouse.x - currentMouse.x;

        if (deltaLeft > 0f)
        {
            float drop = deltaLeft * ScrewSpeed;

            Vector2 pos = WaterBottle.anchoredPosition;
            pos.y -= drop;
            WaterBottle.anchoredPosition = pos;

            _droppedAmount += drop;

            if (_droppedAmount >= RequiredDropDistance)
            {
                _taskFinished = true;
                _isHolding = false;

                _ctx.TaskManager.CurrentTask.OnTaskEnd(true);

                ResetTaskState();
                return;
            }
        }

        _previousMouse = currentMouse;
    }

    private void ResetTaskState()
    {
        _isHolding = false;
        _taskFinished = false;
        _droppedAmount = 0f;

        if (WaterBottle != null)
            WaterBottle.anchoredPosition = _startPosition;
    }
}