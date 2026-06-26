using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Quota : MonoBehaviour
{
    RectTransform _parentRect;
    public Image _img;
    public TMP_Text _text;
    public int quotaAmount = 10;
    public int batteriesDroppedCount = 0;

    private void Start()
    {
        _img = GetComponent<Image>();
        _parentRect = transform.parent.gameObject.GetComponent<RectTransform>();
        Debug.Log("Parent rect: " + _parentRect.name);
    }

    public void DropOff()
    {
        if (_img.fillAmount >= 1f) return;
        else _img.fillAmount += 1f / (float)quotaAmount;
        batteriesDroppedCount++;
        _parentRect.DOShakeScale(1f, 0.3f).SetEase(Ease.InOutBounce);
        if (_img.fillAmount >= 1f)
        {
            _img.color = Color.green;
            _text.text = "Quota Met!";
        }
    }

    public void ResetQuotaIMG()
    {
        _img.fillAmount = .05f;
        if(ColorUtility.TryParseHtmlString("#FFB069", out Color orangeColor))
        {
            _img.color = orangeColor;
        }
        else
        {
            Debug.LogError("Failed to parse color string.");
        }
        _text.text = "Charging...";
    
    }

    public void ResetDroppedCount()
    {
        batteriesDroppedCount = 0;
        ResetQuotaIMG();
    }

    public void SetQuota(int quotaCount) => quotaAmount = quotaCount;

}
