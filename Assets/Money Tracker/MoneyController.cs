using TMPro;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    MoneyModel _model;
    MoneyView _view;

    int _initialStartingMoney;

    public void Initialize(MoneyControllerData data)
    {
        _model = new();
        _view = new(data.MoneyTexts);

        _initialStartingMoney = data.InitialStartingMoney;

        ResetMoney();
    }    

    public void GainMoney(int amount)
    {
        _model.GainMoney(amount);
        RefreshView();
    }

    /// <summary>
    /// Removes money without checking the affordance. Used for losing money as a punishment.
    /// </summary>
    public void LoseMoney(int amount)
    {
        _model.LoseMoney(amount);
        RefreshView();
    }

    /// <summary>
    /// Removes money after checking the affordance. Used for spending money on items.
    /// </summary>
    public void SpendMoney(int amount)
    {
        if(!CanAfford(amount)) 
            return;
        
        LoseMoney(amount);
    }

    public void ResetMoney()
    {
        _model.SetMoney(_initialStartingMoney);
        RefreshView();
    }
    public int CurrentMoney() => _model.CurrentMoney();
    public bool CanAfford(int amount) => _model.CanAfford(amount);
    void RefreshView() => _view.RefreshView(_model.CurrentMoney());
}

[System.Serializable]
public struct MoneyControllerData
{
    public int InitialStartingMoney;
    public TextMeshProUGUI[] MoneyTexts;
}
