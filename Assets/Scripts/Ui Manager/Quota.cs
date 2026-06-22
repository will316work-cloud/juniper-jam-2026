using UnityEngine;
using UnityEngine.UI;

public class Quota : MonoBehaviour
{
    public Image _img;
    public int quotaAmount = 10;
    public bool _debug = false;

    private void Start()
    {
        _img = GetComponent<Image>();
    }

    public void DropOff()
    {
        if (_img.fillAmount >= 1f) return;
        else _img.fillAmount += 1f / (float)quotaAmount;
    }

    public void ResetQuotaIMG()
    {
        _img.fillAmount = .1f;
    }

    public void SetQuota(int quotaCount) => quotaAmount = quotaCount;

    private void Update()
    {
        if (_debug) DropOff();
    }
}
