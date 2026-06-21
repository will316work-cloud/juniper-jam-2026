using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField] private GameObject batteryVisual;
    public int moneyPerDropoff;
    public int healthPerDropoff;
    public int rotationsPerFill;
    public int rotationsSoFar = 0;
    public bool _isFilled;

    public void ChargeBattery()
    {
        if (_isFilled) return;
        rotationsSoFar++;
        IncreaseVisualFill();
        if (rotationsSoFar >= rotationsPerFill) _isFilled = true;
    }

    private void IncreaseVisualFill()
    {
        if (_isFilled) return;
        Debug.Log("Battery is " + (float)rotationsSoFar / (float)rotationsPerFill * 100 + " percent full");
        //visual element changes here (soFar / perFill) amount
    }

    private void DecreaseVisualFill()
    {
        if (!_isFilled) return;
        Debug.Log("New Battery Visual");
        //visual element changes here (soFar / perFill) amount
    }

    public void SwapBattery()
    {
        if(!_isFilled) return;
        DecreaseVisualFill();
        Debug.Log("Battery swapped");
        rotationsSoFar = 0;
        _isFilled = false;
    }

}
