using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public InGameUiHandler InGameUiHandler = new();
    public GameOverUiHandler GameOverUiHandler = new();
    public IngameMenuHandler IngameMenuHandler = new();

    public MainMenuHandler MainMenuHandler = new();
    public CreditsHandler CreditsHandler = new();

    public void Initialize(GameContext ctx, UiManagerContext uiManagerContext)
    {
        InGameUiHandler.Initialize(ctx, uiManagerContext.InGameUiHandlerData);
        GameOverUiHandler.Initialize(ctx, uiManagerContext.GameOverUiHandlerData);
        IngameMenuHandler.Initialize(ctx, uiManagerContext.IngameMenuHandlerData);

        MainMenuHandler.Initialize(ctx, uiManagerContext.MainMenuHandlerData);
        CreditsHandler.Initialize(ctx, uiManagerContext.CreditsHandlerData);

        InitializeButtons(ctx);
    }

    void InitializeButtons(GameContext ctx)
    {
        ButtonScript[] buttons = FindObjectsByType<ButtonScript>(FindObjectsInactive.Include);

        foreach (ButtonScript button in buttons)
            button.Initialize(ctx.PoolManager.SfxPooler);

        Debug.Log("Button count: " + buttons.Length);
    }
    private void Update()
    {
        IngameMenuHandler.Tick();
        InGameUiHandler.Tick();
    }
}

[System.Serializable]
public class UiManagerContext
{
    public InGameUiHandlerData InGameUiHandlerData;
    public GameOverUiHandlerData GameOverUiHandlerData;
    public IngameMenuHandlerData IngameMenuHandlerData;

    public MainMenuHandlerData MainMenuHandlerData;
    public CreditsHandlerData CreditsHandlerData;
}