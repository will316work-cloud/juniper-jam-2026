using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class ToiletPaperPly : TaskProp
{
    public Transform PaperPly;
    public float PlySpeed = 0.2f;
    public float DistanceNeededForWin;

    private Vector3 _initialPosition;
    private float _distanceTraveled;
    private bool _isDragging;
    private Vector3 _currentMousePosition;
    private Vector3 _previousMousePosition;

    public override void Initialize(GameContext ctx)
    {
        base.Initialize(ctx);
        _initialPosition = PaperPly.localPosition;   
    }

    public override void OnTaskStart()
    {
        _isDragging = false;
        _distanceTraveled = 0;
        PaperPly.localPosition = _initialPosition;
    }

    public override void PointerDownHandler()
    {
        _isDragging = true;
        _previousMousePosition = Mouse.current.position.ReadValue();
    }

    public override void PointerUpHandler()
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
            if(!_ctx.PoolManager.TaskSoundHandler.IsPlaying)
                _ctx.PoolManager.TaskSoundHandler.StartTaskSound(TaskSoundType.ToiletPaper);

            PaperPly.localPosition -= new Vector3(0, delta * PlySpeed, 0);
            _distanceTraveled += delta * PlySpeed;

            if(_distanceTraveled >= DistanceNeededForWin)
                _ctx.TaskManager.CurrentTask.OnTaskEnd(true); 
        }

        _previousMousePosition = _currentMousePosition;
    }
}