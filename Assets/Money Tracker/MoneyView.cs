using TMPro;

public class MoneyView
{
    private TextMeshProUGUI _moneyText;

    public MoneyView(TextMeshProUGUI moneyText) => _moneyText = moneyText;

    public void RefreshView(int amount) => _moneyText.text = amount.ToString();
}