using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Vector3 movementDirPivot;
    private Rigidbody rb;
    private GameInput gameInput;


    public void Instantiate()
    {
        rb = GetComponentInChildren<Rigidbody>();


    }

    private void LateUpdate()
    {
    }
}
