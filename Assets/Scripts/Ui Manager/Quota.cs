using UnityEngine;
using UnityEngine.UI;

public class Quota : MonoBehaviour
{
    private Image _img;
    public int quotaAmount = 10;

    private void Start()
    {
        _img = GetComponent<Image>();
    }

    public void DropOff()
    {
        if (_img.fillAmount >= 1f) return;
        else _img.fillAmount += 1f / (float)quotaAmount;
    }

    public void SetQuota(int quotaCount) => QuotaAmount = quotaCount;
}
