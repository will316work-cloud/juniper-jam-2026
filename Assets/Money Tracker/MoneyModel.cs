public class MoneyModel
{
    private int _money = 0;

    public void GainMoney(int amount) => _money += amount;
    public void LoseMoney(int amount)
    {
        _money -= amount;
        if(_money < 0) _money = 0;
    }
    public int CurrentMoney() => _money;  
    public bool CanAfford(int amount) => _money >= amount;
}