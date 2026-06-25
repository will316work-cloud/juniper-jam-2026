using UnityEngine;

public class WheelCatchup : MonoBehaviour
{
    [SerializeField] private GameObject _character;

    private void Update()
    {
        if (_character != null)
        {
            Vector3 targetPosition = new Vector3(_character.transform.position.x, transform.position.y, _character.transform.position.z);
            transform.position = targetPosition;
        }
    }
}
