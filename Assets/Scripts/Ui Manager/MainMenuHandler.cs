using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : IUiHandler
{
    private GameObject _panel;
    private Button _playButton;
    private Button _creditsButton;
    private Button _quitButton;
    private Button _settingsButton;

    private GameContext _ctx;

    public void Initialize(GameContext ctx, MainMenuHandlerData data)
    {
        _ctx = ctx;

        _panel = data.MainMenuPanel;

        _playButton = data.PlayButton;
        _creditsButton = data.CreditsButton;
        _quitButton = data.QuitButton;
        _settingsButton = data.SettingsButton;

        _playButton.onClick.AddListener(PlayButtonHandler);
        _creditsButton.onClick.AddListener(CreditsButtonHandler);
        _quitButton.onClick.AddListener(QuitButtonHandler);
        _settingsButton.onClick.AddListener(SettingsButtonHandler);

        SetPanelState(false);
    }

    public void SetPanelState(bool state)
    {
        _panel.SetActive(state);
    }

    public bool IsPanelActive()
    {
        return _panel.activeSelf;
    }

    private void PlayButtonHandler()
    {
        _ctx.GameStateController.ChangeState(StateType.Gameplay);
    }

    private void CreditsButtonHandler()
    {
        _ctx.UiManager.CreditsHandler.SetPanelState(true);
    }

    private void SettingsButtonHandler()
    {
        SetPanelState(false);

        _ctx.UiManager.IngameMenuHandler.SetPanelState(true);
    }

    private void QuitButtonHandler()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

[System.Serializable]
public class MainMenuHandlerData
{
    public GameObject MainMenuPanel;

    public Button PlayButton;
    public Button CreditsButton;
    public Button SettingsButton;
    public Button QuitButton;
}