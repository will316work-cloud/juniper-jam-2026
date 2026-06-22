using TMPro;
using UnityEngine;

public class DebugBatteryUI : MonoBehaviour
{
    private TMP_Text _text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void ChangeBatteryText(int currentBattery)
    {
        _text.text = $"Battery: {currentBattery}/10";
    }
}
