using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quota : MonoBehaviour
{
    public Image _img;
    public TMP_Text _text;
    public int quotaAmount = 10;
    public int batteriesDroppedCount = 0;

    private void Start()
    {
        _img = GetComponent<Image>();
    }

    public void DropOff()
    {
        if (_img.fillAmount >= 1f) return;
        else _img.fillAmount += 1f / (float)quotaAmount;
        batteriesDroppedCount++;
        if (_img.fillAmount >= 1f)
        {
            _img.color = Color.green;
            _text.text = "Quota Met!";
        }
    }

    public void ResetQuotaIMG()
    {
        _img.fillAmount = .05f;
        if(ColorUtility.TryParseHtmlString("#00FFF8", out Color blueColor))
        {
            _img.color = blueColor;
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
