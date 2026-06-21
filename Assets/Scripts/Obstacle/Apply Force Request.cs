using UnityEngine;

[CreateAssetMenu(fileName = "Apply Force Request", menuName = "Scriptable Objects/Apply Force Request")]
public class ApplyForceRequest : ScriptableObject
{
    #region Serialized Fields


    [Header("Force Magnitude")]
    [SerializeField] private float _forcePower;
    [SerializeField] private ForceMode _forceMode;

    [Header("Force Direction")]
    [SerializeField] private Space _directionMode;
    [SerializeField] private DirectionOrigin _directionOrigin;
    [SerializeField] private Vector3 _eulerAngleDeviation;


    #endregion

    private enum DirectionOrigin
    {
        Right,
        Left,
        Up,
        Down,
        Forward,
        Backward
    }

    #region Public Methods


    public void ApplyForceFromRequest(Rigidbody rb, float forceMultiplier = 1f)
    {
        if (rb == null)
            return;

        // Direction Vector Calculation

        Vector3 directionVector = Vector3.zero;

        switch (_directionOrigin)
        {
            case DirectionOrigin.Right:
                directionVector = _directionMode == Space.Self ? rb.transform.right : Vector3.right;
                break;

            case DirectionOrigin.Left:
                directionVector = _directionMode == Space.Self ? rb.transform.right * -1 : Vector3.left;
                break;

            case DirectionOrigin.Up:
                directionVector = _directionMode == Space.Self ? rb.transform.up : Vector3.up;
                break;

            case DirectionOrigin.Down:
                directionVector = _directionMode == Space.Self ? rb.transform.up * -1 : Vector3.down;
                break;

            case DirectionOrigin.Forward:
                directionVector = _directionMode == Space.Self ? rb.transform.forward : Vector3.forward;
                break;

            case DirectionOrigin.Backward:
                directionVector = _directionMode == Space.Self ? rb.transform.forward * -1 : Vector3.back;
                break;

            default:
                break;
        }

        directionVector = Quaternion.Euler(_eulerAngleDeviation) * directionVector;

        // Force Application
        rb.AddForce(_forcePower * forceMultiplier * directionVector, _forceMode);
    }


    #endregion
}
