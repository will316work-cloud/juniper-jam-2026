using TMPro;
using UnityEngine;

public class MoneyController : MonoBehaviour
{

    public static MoneyController Instance;
    [SerializeField] MoneyControllerData _data;

    MoneyModel _model;
    MoneyView _view;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _model = new();
        _view = new(_data.MoneyText);

        GainMoney(_data.InitialStartingMoney);
    }    

    public void GainMoney(int amount)
    {
        _model.GainMoney(amount);
        _view.GainMoney(_model.CurrentMoney());
    }

    /// <summary>
    /// Removes money without checking the affordance. Used for losing money as a punishment.
    /// </summary>
    public void LoseMoney(int amount)
    {
        _model.LoseMoney(amount);
        _view.LoseMoney(_model.CurrentMoney());
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

    public int CurrentMoney() => _model.CurrentMoney();
    public bool CanAfford(int amount) => _model.CanAfford(amount);
}

[System.Serializable]
public struct MoneyControllerData
{
    public int InitialStartingMoney;
    public TextMeshProUGUI MoneyText;
}
