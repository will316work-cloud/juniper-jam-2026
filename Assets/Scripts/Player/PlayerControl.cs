using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float accelSpeed, rotSpeed, maxSpeed;
    [SerializeField] private GameObject movementDirPivotObj;
    [SerializeField] private Battery battery;
    private float _rotationDeltaSum = 0f;
    private Rigidbody rb;
    private GameInput gameInput;
    private GameObject playerCollisionObject;
    Vector3 _camForward;
    Vector3 _camRight;


    public void Instantiate(Rigidbody rigidbody, GameInput input, GameObject collisionObject)
    {
        rb = rigidbody;
        gameInput = input;
        playerCollisionObject = collisionObject;

        //setting and normalizing movement directions
        _camForward = movementDirPivotObj.transform.forward;
        _camRight = movementDirPivotObj.transform.right;
        _camForward.y = 0;
        _camRight.y = 0;
        _camForward.Normalize();
        _camRight.Normalize();
    }

    private void LateUpdate()
    {
        if (gameInput.moveInput != Vector2.zero) { 

            //reading input
            float _forwardInput = gameInput.moveInput.y;
            float _rightInput = gameInput.moveInput.x;

            //setting direction depending on input and camera orientation
            Vector3 movementDir = ((_camForward * _forwardInput) + (_camRight * _rightInput));

            //movement call
            rb.AddForce(movementDir * accelSpeed, ForceMode.Acceleration);

            //speed cap
            if(rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }

            //calculating target rotation. calculated early to be able to use it for rotation delta calculation
            Quaternion targetRot = Quaternion.LookRotation(movementDir);
            Quaternion targetRotThisTick = Quaternion.Slerp(playerCollisionObject.transform.rotation, targetRot, rotSpeed * Time.deltaTime);

            //rotation delta calculation and logging
            Vector3 playerEuler = playerCollisionObject.transform.rotation * Vector3.forward;
            Vector3 targetEuler = targetRotThisTick * Vector3.forward;
            float rotationDelta = Vector3.SignedAngle(playerEuler, targetEuler, Vector3.up);

            //if you change rotation direction, reset the sum, otherwise it takes up to two rotations to trigger another full rotation
            if(Mathf.Sign(rotationDelta) != Mathf.Sign(_rotationDeltaSum))
            {
                _rotationDeltaSum = 0f;
            }
            _rotationDeltaSum += rotationDelta;

            //full rotation check
            if (_rotationDeltaSum > 360f || _rotationDeltaSum < -360f)
            {
                //Debug.Log("Full Rotation: " + _rotationDeltaSum);
                _rotationDeltaSum = 0f;
                battery.ChargeBattery();
            }

            //applying rotation
            playerCollisionObject.transform.rotation = targetRotThisTick;

        }
    }

}
