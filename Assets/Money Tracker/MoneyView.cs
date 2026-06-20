using TMPro;

public class MoneyView
{
    private TextMeshProUGUI[] _moneyTexts;

    public MoneyView(TextMeshProUGUI[] moneyTexts) => _moneyTexts = moneyTexts;

    public void RefreshView(int amount)
    {
        foreach (var moneyText in _moneyTexts)
        {
            moneyText.text = amount.ToString();
        }
    }
}