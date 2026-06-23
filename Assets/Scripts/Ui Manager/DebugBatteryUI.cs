using TMPro;
using UnityEngine;

public class DebugBatteryUI : MonoBehaviour
{
    [SerializeField]private TMP_Text _text;
    

    public void ChangeBatteryText(int currentBattery)
    {
        _text.text = $"Battery: {currentBattery}/10";
    }
}
