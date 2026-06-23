using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUiHandler : IUiHandler
{
    GameObject _panel;
    GameObject _interactionIndicatorObj;
    GameContext _ctx;
    Button _ingameMenuButton;

    public bool IsPanelActive() => _panel.activeSelf;
    public void Initialize(GameContext ctx, InGameUiHandlerData data)
    {
        _ctx = ctx;

        _ingameMenuButton = data.IngameMenuButton;
        _interactionIndicatorObj = data.InteractionIndicatorObj;
        _panel = data.InGameUiPanel;

        _ingameMenuButton.onClick.AddListener(() => IngameMenuButtonHandler());

        SetPanelState(false);
    }

    void IngameMenuButtonHandler()
    {
        Time.timeScale = 0f;
        SetPanelState(false);
        _ctx.UiManager.IngameMenuHandler.SetPanelState(false);
        _ctx.UiManager.IngameMenuHandler.SetPanelState(true);
    }
    
    public void SetPanelState(bool state) => _panel.SetActive(state);

    public void SetInteractionIndicatorstate(bool state) => _interactionIndicatorObj.SetActive(state);
    public bool IndicatorState() => _interactionIndicatorObj.activeSelf;
}

[System.Serializable]
public class InGameUiHandlerData
{
    public GameObject InGameUiPanel;
    public GameObject InteractionIndicatorObj;
    public Button IngameMenuButton;
}