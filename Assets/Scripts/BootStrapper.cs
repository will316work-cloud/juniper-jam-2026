using UnityEngine;

public class BootStrapper : MonoBehaviour
{
    [Header("System references")]
    public GameContext GameContext;

    [Space]
    [Header("Dependency references")]
    public MoneyControllerData MoneyControllerData;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        GameContext.TaskManager.Initialize(GameContext);
        GameContext.MoneyController.Initialize(MoneyControllerData);
    }
}