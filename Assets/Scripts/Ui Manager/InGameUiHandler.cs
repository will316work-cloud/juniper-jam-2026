using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUiHandler : IUiHandler
{
    GameObject _panel;
    GameObject _interactionIndicatorObj;
    GameContext _ctx;
    Button _ingameMenuButton;
    Image _playerImage;
    Dictionary<PlayerState, Sprite> _playerStates = new();
    PlayerState _currentPlayerState;

    public bool IsPanelActive() => _panel.activeSelf;
    public void Initialize(GameContext ctx, InGameUiHandlerData data)
    {
        _ctx = ctx;

        _ingameMenuButton = data.IngameMenuButton;
        _interactionIndicatorObj = data.InteractionIndicatorObj;
        _panel = data.InGameUiPanel;
        _playerImage = data.PlayerImage;

        foreach(PlayerStateEntry entry in data.PlayerStates)
            _playerStates.Add(entry.PlayerState, entry.Icon);

        _ingameMenuButton.onClick.AddListener(() => ctx.UiManager.IngameMenuHandler.OnMenuOpen());

        SetPanelState(false);
    }

    // void IngameMenuButtonHandler()
    // {
    //     CursorHandler.SetCursorVisible(true);
    //     Time.timeScale = 0f;
    //     SetPanelState(false);
    //     _ctx.UiManager.IngameMenuHandler.SetPanelState(true);
    // }
    
    public void SetPanelState(bool state) => _panel.SetActive(state);
    public void SetInteractionIndicatorstate(bool state) => _interactionIndicatorObj.SetActive(state);
    public bool IndicatorState() => _interactionIndicatorObj.activeSelf;
    public void Tick()
    {
        float health = _ctx.WorldHealthMeter.CurrentHealth;
        float max = _ctx.WorldHealthMeter.MaxHealth;

        PlayerState newState;
        if (health >= max / 1.5f)
            newState = PlayerState.Normal;
        else if (health >= max / 3f)
            newState = PlayerState.Worried;
        else
            newState = PlayerState.Panic;

        if (newState == _currentPlayerState) return;
        _currentPlayerState = newState;
        _playerImage.sprite = _playerStates[_currentPlayerState];
    }
}

[System.Serializable]
public class InGameUiHandlerData
{
    public GameObject InGameUiPanel;
    public GameObject InteractionIndicatorObj;
    public Button IngameMenuButton;
    public Image PlayerImage;
    public PlayerStateEntry[] PlayerStates;
}

[System.Serializable]
public class PlayerStateEntry
{
    public PlayerState PlayerState;
    public Sprite Icon;
}