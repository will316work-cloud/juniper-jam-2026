using UnityEngine;

public class TitlePathPoint : MonoBehaviour
{
    public bool IsAvailable = true;
    private Vector3 _position; public Vector3 Position => _position;

    public void Initialize()
    {
        _position = transform.position;
    }
}