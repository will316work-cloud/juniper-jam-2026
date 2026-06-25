using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class IngameMenuHandler : IUiHandler
{
    private GameObject _panel;

    private Button _mainMenuButton;
    private Button _restartButton;
    private Button _resumeButton;

    [Range(0,100)] private Slider _masterVolumeSlider;
    [Range(0,100)] private Slider _sfxVolumeSlider;
    [Range(0,100)] private Slider _musicVolumeSlider;
    [Range(60,100)] private Slider _fpsSlider;

    private Toggle _vsyncToggle;
    private TMP_Dropdown _resolutionDropdown;
    private TextMeshProUGUI _fpsText;

    GameContext _ctx;
    public bool CanOpenIngameMenu;
    public void SetCanOpenInGameMenueState(bool state) => CanOpenIngameMenu = state;

    public bool IsPanelActive() => _panel.activeSelf;
    public void SetPanelState(bool state) => _panel.SetActive(state);

    public void Initialize(GameContext ctx, IngameMenuHandlerData data)
    {
        _ctx = ctx;

        _panel = data.Panel;
        _mainMenuButton = data.MainMenuButton;
        _restartButton = data.RestartButton;
        _resumeButton = data.ResumeButton;

        _masterVolumeSlider = data.MasterVolumeSlider;
        _sfxVolumeSlider = data.SfxVolumeSlider;
        _musicVolumeSlider = data.MusicVolumeSlider;
        _fpsSlider = data.FpsSlider;
        _vsyncToggle = data.VsyncToggle;
        _resolutionDropdown = data.ResolutionDropdown;

        _masterVolumeSlider.maxValue = 100;
        _masterVolumeSlider.minValue = 0;

        _sfxVolumeSlider.maxValue = 100;
        _sfxVolumeSlider.minValue = 0;

        _musicVolumeSlider.maxValue = 100;
        _musicVolumeSlider.minValue = 0;

        _fpsSlider.maxValue = 144;
        _fpsSlider.minValue = 60;

        _fpsText = data.FpsSliderText;

        SetDropdownValues();

        _mainMenuButton.onClick.AddListener(() => MainMenuButtonClickHandler());
        _restartButton.onClick.AddListener(() => RestartButtonClickHandler());
        _resumeButton.onClick.AddListener(() => ResumeButtonClickHandler());

        _masterVolumeSlider.value = _ctx.PoolManager.MasterVolume * 100;
        _sfxVolumeSlider.value = _ctx.PoolManager.OverallVolume_SFX * 100;
        _musicVolumeSlider.value = _ctx.PoolManager.OverallVolume_Song * 100;
        _vsyncToggle.isOn = _ctx.SettingsData.Vsync;
        _fpsSlider.value = _ctx.SettingsData.FpsCap;
        _resolutionDropdown.value = _ctx.SettingsData.ResolutionIndex;

        _masterVolumeSlider.onValueChanged.AddListener(HandleMasterVolumeChange);
        _sfxVolumeSlider.onValueChanged.AddListener(HandleSfxVolumeChange);
        _musicVolumeSlider.onValueChanged.AddListener(HandleMusicVolumeChange);

        _fpsSlider.onValueChanged.AddListener(HandleFpsSliderChange);
        _vsyncToggle.onValueChanged.AddListener(HandleVsyncToggleValueChange);
        _resolutionDropdown.onValueChanged.AddListener(HandleResolutionChange);
        _fpsText.text = "FPS Cap - " + (int)_ctx.SettingsData.FpsCap;

        SetPanelState(false);
    }

    private void RestartButtonClickHandler()
    {
        _ctx.Quota.ResetDroppedCount();
        _ctx.PlayerControl.AddMovementBlockReason(MovementBlockReason.Menu);
        _ctx.UiManager.GameOverUiHandler.SetPanelState(false);
        _ctx.DifficultyManager.SetDifficulty(1);
        _ctx.PlayerControl.TeleportPlayerToStartingPosition();
        _ctx.Battery.ResetBatteryFill();
        _ctx.WorldHealthMeter.ResetHealth();
        _ctx.MoneyController.ResetMoney();
        _ctx.CoworkerManager.TeleportCoworkersToOriginalPlace();
        _ctx.DayTimeController.ResetTime();
        _ctx.DayTimeController.ResetDay();

        SetPanelState(false);
        Time.timeScale = 1f;
        _ctx.GameStateController.ChangeState(StateType.Gameplay);
    }

    public void OnMenuOpen()
    {
        RefreshSettings();
        _ctx.UiManager.InGameUiHandler.SetPanelState(false);
        SetPanelState(true);
        Time.timeScale = 0f;
        CursorHandler.SetCursorVisible(true);
    }

    private void RefreshSettings()
    {
        _masterVolumeSlider.SetValueWithoutNotify(_ctx.PoolManager.MasterVolume * 100);
        _sfxVolumeSlider.SetValueWithoutNotify(_ctx.PoolManager.OverallVolume_SFX * 100);
        _musicVolumeSlider.SetValueWithoutNotify(_ctx.PoolManager.OverallVolume_Song * 100);
        _vsyncToggle.SetIsOnWithoutNotify(_ctx.SettingsData.Vsync);
        _fpsSlider.SetValueWithoutNotify(_ctx.SettingsData.FpsCap);
        _resolutionDropdown.SetValueWithoutNotify(_ctx.SettingsData.ResolutionIndex);
        _fpsText.text = "FPS Cap - " + (int)_ctx.SettingsData.FpsCap;
    }

    public void Tick()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame && CanOpenIngameMenu)
            if(!IsPanelActive()) OnMenuOpen();
            else ResumeButtonClickHandler();
    }

    private void MainMenuButtonClickHandler()
    {
        _ctx.PlayerControl.AddMovementBlockReason(MovementBlockReason.Menu);
        SetPanelState(false);
        _ctx.GameStateController.ChangeState(StateType.MainMenu);
    }

    private void ResumeButtonClickHandler()
    {
        CursorHandler.SetCursorVisible(false);
        SetPanelState(false);
        _ctx.UiManager.InGameUiHandler.SetPanelState(true);
        Time.timeScale = 1f;
    }

    void HandleVsyncToggleValueChange(bool value)
    {
        _ctx.SettingsData.Vsync = value;
    }
    void HandleMasterVolumeChange(float value)
    {
        float volume;

        if(_masterVolumeSlider.value == 0) 
            volume = 0;
         else
            volume = _masterVolumeSlider.value / 100;

        _ctx.PoolManager.OnMasterVolumeChange(volume);
    }
    void HandleSfxVolumeChange(float value)
    {
        float volume;

        if(_sfxVolumeSlider.value == 0) 
            volume = 0;
         else
            volume = _sfxVolumeSlider.value / 100;

        _ctx.PoolManager.OnSfxVolumeChange(volume);
    }
    void HandleMusicVolumeChange(float value)
    {
        float volume;

        if(_musicVolumeSlider.value == 0) 
            volume = 0;
         else
            volume = _musicVolumeSlider.value / 100;

        _ctx.PoolManager.OnSongVolumeChange(volume);
    }
    void HandleFpsSliderChange(float change)
    {
        _ctx.SettingsData.FpsCap = change;
        _fpsText.text = "FPS Cap - " + (int)change;
    }

    void HandleResolutionChange(int index)
    {
        _ctx.SettingsData.ResolutionIndex = index;
    }

    void SetDropdownValues()
    {
        _resolutionDropdown.options.Clear();

        _resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("854x480 - Windowed"));
        _resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("960x540 - Windowed"));
        _resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("1280x720 - Windowed"));
        _resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("1920x1080 - FullScreenWindow"));
        _resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("2560x1440 - FullScreenWindow"));
        _resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("3840x2160 - FullScreenWindow"));
    }
}

[Serializable]
public class IngameMenuHandlerData
{
    public GameObject Panel;
    public Button MainMenuButton;
    public Button RestartButton;
    public Button ResumeButton;

    public Slider MasterVolumeSlider;
    public Slider SfxVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider FpsSlider;

    public Toggle VsyncToggle;
    public TMP_Dropdown ResolutionDropdown;
    public TextMeshProUGUI FpsSliderText;
}