using UnityEngine;

public class UiManager : MonoBehaviour
{
    public InGameUiHandler InGameUiHandler = new(); 

    public void Initialize(GameContext ctx, UiManagerContext uiManagerContext)
    {
        InGameUiHandler.Initialize(ctx, uiManagerContext.InGameUiHandlerData);
    }
}

[System.Serializable]
public class UiManagerContext
{
    public InGameUiHandlerData InGameUiHandlerData;
}