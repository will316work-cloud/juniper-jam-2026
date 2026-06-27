using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : IUiHandler
{
    private GameObject _mainMenuPanel;
    private GameObject _buttonMenuPanel;
    private GameObject _settingsPanel;
    private GameObject _howToPanel;

    private Button _playButton;
    private Button _howToButton;
    private Button _creditsButton;
    private Button _quitButton;
    private Button _backToMenuFromSettingsButton;
    private Button _backToMenuFromHowToButton;
    private Button _settingsButton;

    private GameContext _ctx;

    [Range(0,100)] private Slider _masterVolumeSlider;
    [Range(0,100)] private Slider _sfxVolumeSlider;
    [Range(0,100)] private Slider _musicVolumeSlider;
    [Range(60,100)] private Slider _fpsSlider;

    private Toggle _vsyncToggle;
    private TMP_Dropdown _resolutionDropdown;
    private TextMeshProUGUI _fpsText;

    public void Initialize(GameContext ctx, MainMenuHandlerData data)
    {
        _ctx = ctx;

        _mainMenuPanel = data.MainMenuPanel;
        _buttonMenuPanel = data.ButtonMenuPanel;
        _settingsPanel = data.SettingsPanel;
        _howToPanel = data.HowToPanel;

        _playButton = data.PlayButton;
        _howToButton = data.HowToButton;
        _creditsButton = data.CreditsButton;
        _quitButton = data.QuitButton;
        _settingsButton = data.SettingsButton;
        _backToMenuFromSettingsButton = data.BackToMenuFromSettingsButton;
        _backToMenuFromHowToButton = data.BackToMenuFromHowToButton;

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

        _playButton.onClick.AddListener(PlayButtonHandler);
        _howToButton.onClick.AddListener(HowToButtonHandler);
        _creditsButton.onClick.AddListener(CreditsButtonHandler);
        _quitButton.onClick.AddListener(QuitButtonHandler);
        _settingsButton.onClick.AddListener(SettingsButtonHandler);
        _backToMenuFromSettingsButton.onClick.AddListener(BeckToMenuFromSettingsButtonHandler);
        _backToMenuFromHowToButton.onClick.AddListener(BackToMenuFromHowToButtonHandler);

        SetPanelState(false);
    }

    public void SetPanelState(bool state)
    {
        if(state)
        {
            SetSettingsPanelState(false);
            SetHowToPanelState(false);
            SetButtonPanelState(true);
        }

        _mainMenuPanel.SetActive(state);
    }
    public void SetButtonPanelState(bool state) => _buttonMenuPanel.SetActive(state);
    public void SetSettingsPanelState(bool state) => _settingsPanel.SetActive(state);
    public void SetHowToPanelState(bool state) => _howToPanel.SetActive(state);
    private void PlayButtonHandler() => _ctx.GameStateController.ChangeState(StateType.Gameplay);
    private void CreditsButtonHandler() => _ctx.UiManager.StartCoroutine(CreditsButtonhandlerRoutine());

    private IEnumerator CreditsButtonhandlerRoutine()
    {
        SetPanelState(false);
        _ctx.CameraController.SwitchToCamera(CameraType.Credits);
        yield return new WaitForSeconds(1.1f);
        _ctx.UiManager.CreditsHandler.SetPanelState(true);
    }

    public bool IsPanelActive() => _mainMenuPanel.activeSelf;

    private void SettingsButtonHandler()
    {
        RefreshSettings();
        SetButtonPanelState(false);
        SetSettingsPanelState(true);
    }

    private void HowToButtonHandler()
    {
        SetButtonPanelState(false);
        SetSettingsPanelState(false);
        SetHowToPanelState(true);
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

    private void BeckToMenuFromSettingsButtonHandler()
    {
        SetSettingsPanelState(false);
        SetButtonPanelState(true);
    }

    private void BackToMenuFromHowToButtonHandler()
    {
        SetHowToPanelState(false);
        SetButtonPanelState(true);
    }

    private void QuitButtonHandler()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
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

[System.Serializable]
public class MainMenuHandlerData
{
    public GameObject MainMenuPanel;
    public GameObject ButtonMenuPanel;
    public GameObject SettingsPanel;
    public GameObject HowToPanel;

    public Button PlayButton;
    public Button HowToButton;
    public Button CreditsButton;
    public Button SettingsButton;
    public Button QuitButton;
    public Button BackToMenuFromSettingsButton;
    public Button BackToMenuFromHowToButton;

    public Slider MasterVolumeSlider;
    public Slider SfxVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider FpsSlider;

    public Toggle VsyncToggle;
    public TMP_Dropdown ResolutionDropdown;
    public TextMeshProUGUI FpsSliderText;

}