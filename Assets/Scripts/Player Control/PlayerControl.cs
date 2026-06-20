using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float accelSpeed, rotSpeed;
    [SerializeField] private GameObject movementDirPivotObj;
    private Rigidbody rb;
    private GameInput gameInput;
    private GameObject playerCollisionObject;


    public void Instantiate(Rigidbody rigidbody, GameInput input, GameObject collisionObject)
    {
        rb = rigidbody;
        gameInput = input;
        playerCollisionObject = collisionObject;
    }

    private void LateUpdate()
    {
        if (gameInput.moveInput != Vector2.zero) { 
            //setting and normalizing movement directions
            Vector3 _camForward = movementDirPivotObj.transform.forward;
            Vector3 _camRight = movementDirPivotObj.transform.right;
            _camForward.y = 0;
            _camRight.x = 0;

            //reading input
            float _forwardInput = gameInput.moveInput.y;
            float _rightInput = gameInput.moveInput.x;

            //setting direction depending on input and camera orientation
            Vector3 movementDir = (_camForward * _forwardInput + _camRight * _rightInput);


            Quaternion targetRot = Quaternion.LookRotation(movementDir);
            
        }
    }

}
