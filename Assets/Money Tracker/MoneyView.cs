using TMPro;

public class MoneyView
{
    private TextMeshProUGUI _moneyText;

    public MoneyView(TextMeshProUGUI moneyText) => _moneyText = moneyText;

    public void GainMoney(int amount)
    {
        _moneyText.text = amount.ToString();
    }

    public void LoseMoney(int amount)
    {
        _moneyText.text = amount.ToString();
    }

}