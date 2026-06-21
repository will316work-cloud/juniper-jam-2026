using UnityEngine;

public class ApplyForceRequestManager : MonoBehaviour
{
    #region Serialized Fields


    [SerializeField] private Rigidbody _forceTarget;
    [SerializeField] private ApplyForceRequest _storedRequest;
    [Space]
    [SerializeField] private float _forceMultiplier = 1.0f;

    [Header("Apply Force Event Toggles")]
    [SerializeField] private bool _onAwakeApply;
    [SerializeField] private bool _onStartApply;
    [SerializeField] private bool _onEnableApply;


    #endregion

    #region Private Fields


    private bool _canRequestOnEvent = true;


    #endregion

    #region MonoBehavior Callbacks


    private void OnValidate()
    {
        if (_forceTarget == null)
            _forceTarget = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        if (_canRequestOnEvent && _onAwakeApply)
        {
            ApplyRequest();
            _canRequestOnEvent = false;
        }
    }

    private void Start()
    {
        if (_canRequestOnEvent && _onStartApply)
        {
            ApplyRequest();
            _canRequestOnEvent = false;
        }
    }

    private void OnEnable()
    {
        if (_canRequestOnEvent && _onEnableApply)
        {
            ApplyRequest();
            _canRequestOnEvent = false;
        }
    }

    private void OnDisable()
    {
        _canRequestOnEvent = true;
    }


    #endregion

    #region Public Methods


    public void ApplyRequest(ApplyForceRequest request)
    {
        if (_forceTarget == null || request == null)
            return;

        request.ApplyForceFromRequest(_forceTarget, _forceMultiplier);
    }

    [ContextMenu("Apply Request")]
    public void ApplyRequest()
    {
        ApplyRequest(_storedRequest);
    }


    #endregion
}
