using UnityEngine;

public class ApplyForceRequestManager : MonoBehaviour
{
    #region Serialized Fields


    [SerializeField] private Rigidbody _forceTarget;
    [SerializeField] private ApplyForceRequest _storedRequest;
    [Space]
    [SerializeField] private float _forceMultiplier = 1.0f;


    #endregion

    #region MonoBehavior Callbacks


    private void OnValidate()
    {
        if (_forceTarget == null)
            _forceTarget = GetComponent<Rigidbody>();
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
