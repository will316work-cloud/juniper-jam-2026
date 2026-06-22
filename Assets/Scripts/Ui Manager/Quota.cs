using UnityEngine;
using UnityEngine.UI;

public class Quota : MonoBehaviour
{
    private Image _img;
    public int QuotaAmount = 10;

    private void Start()
    {
        _img = GetComponent<Image>();
    }

    public void DropOff()
    {
        if (_img.fillAmount >= 1f) return;
        else _img.fillAmount += 1f / (float)QuotaAmount;
    }
}
