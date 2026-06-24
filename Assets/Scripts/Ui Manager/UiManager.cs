using UnityEngine;

public class UiManager : MonoBehaviour
{
    public InGameUiHandler InGameUiHandler = new(); 
    public GameOverUiHandler GameOverUiHandler = new();
    public IngameMenuHandler IngameMenuHandler = new(); 

    public void Initialize(GameContext ctx, UiManagerContext uiManagerContext)
    {
        InGameUiHandler.Initialize(ctx, uiManagerContext.InGameUiHandlerData);
        GameOverUiHandler.Initialize(ctx, uiManagerContext.GameOverUiHandlerData);
        IngameMenuHandler.Initialize(ctx, uiManagerContext.IngameMenuHandlerData);
    }

    void Update()
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
}