using TMPro;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    MoneyModel _model;
    MoneyView _view;

    int _initialStartingMoney;
    MoneyControllerData _data;

    public void Initialize(MoneyControllerData data)
    {
        _model = new();
        _view = new(data.MoneyTexts);
        _data = data;

        _initialStartingMoney = data.InitialStartingMoney;

        ResetMoney();
    }    

    public void SetPanelState(bool state)
    {
        _view.SetPanelState(state);
    }

    public void GainMoney(int amount)
    {
        _model.GainMoney(amount);
        UiEffectHandler.BounceText(Type.Out, _data.MoneyTexts[0], 0.5f, 8f,EffectColorChangeType.Green);
        RefreshView();
    }

    /// <summary>
    /// Removes money without checking the affordance. Used for losing money as a punishment.
    /// </summary>
    public void LoseMoney(int amount)
    {
        _model.LoseMoney(amount);
        UiEffectHandler.BounceText(Type.Shake, _data.MoneyTexts[0], 0.5f, 8f, EffectColorChangeType.Red);
        UiEffectHandler.BounceText(Type.Out, _data.MoneyTexts[0], 0.5f, 8f);
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
