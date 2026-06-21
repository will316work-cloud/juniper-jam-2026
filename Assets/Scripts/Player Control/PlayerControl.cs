using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float accelSpeed, rotSpeed, maxSpeed;
    [SerializeField] private GameObject movementDirPivotObj;
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

            rb.AddForce(movementDir * accelSpeed, ForceMode.Acceleration);

            if(rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }

            Quaternion targetRot = Quaternion.LookRotation(movementDir);
            playerCollisionObject.transform.rotation = Quaternion.Slerp(playerCollisionObject.transform.rotation, targetRot, rotSpeed * Time.deltaTime);

        }
    }

}
