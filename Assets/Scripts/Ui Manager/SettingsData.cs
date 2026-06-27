using UnityEngine;

public class SettingsData
{
    private float _fpsCap;
    private bool _vsync;
    private int _resolutionIndex;
    public bool _isDayFiveCleared; public bool IsDayFiveCleared => _isDayFiveCleared;
    GameObject _employeeOfTheMonthObj;

    public void CheckIfLevelFiveIsCleared()
    {
        _isDayFiveCleared = PlayerPrefs.GetInt("levelFiveCleared", 0) == 1;

        if(_isDayFiveCleared) 
            _employeeOfTheMonthObj.SetActive(true);
        else 
            _employeeOfTheMonthObj.SetActive(false);
    }
    public void SetLevelFiveCleared()
    {
        PlayerPrefs.SetInt("levelFiveCleared", 1);
    }

    public float FpsCap
    {
        get => _fpsCap;
        set { _fpsCap = value; Application.targetFrameRate = (int)value; }
    }

    public bool Vsync
    {
        get => _vsync;
        set { _vsync = value; QualitySettings.vSyncCount = value ? 1 : 0; }
    }

    public int ResolutionIndex
    {
        get => _resolutionIndex;
        set { _resolutionIndex = value; ApplyResolution(value); }
    }

    public SettingsData(StartingSettings startingSettings)
    {
        _employeeOfTheMonthObj = startingSettings.EmployeeOfTheMonthObj;
        CheckIfLevelFiveIsCleared();

        FpsCap = startingSettings.FpsCap;
        Vsync = startingSettings.Vsync;
        ResolutionIndex = (int)startingSettings.Resolution;
    }

    private static void ApplyResolution(int index)
    {
        switch (index)
        {
            case 0: Screen.SetResolution(854,  480,  FullScreenMode.Windowed); break;
            case 1: Screen.SetResolution(960,  540,  FullScreenMode.Windowed); break;
            case 2: Screen.SetResolution(1280, 720,  FullScreenMode.Windowed); break;
            case 3: Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow); break;
            case 4: Screen.SetResolution(2560, 1440, FullScreenMode.FullScreenWindow); break;
            case 5: Screen.SetResolution(3840, 2160, FullScreenMode.FullScreenWindow); break;
        }
    }
}

public enum ResolutionOption
{
    _854x480_Windowed = 0,
    _960x540_Windowed = 1,
    _1280x720_Windowed = 2,
    _1920x1080_FullScreen = 3,
    _2560x1440_FullScreen = 4,
    _3840x2160_FullScreen = 5,
}

[System.Serializable]
public class StartingSettings
{
    public float FpsCap;
    public bool Vsync;
    public ResolutionOption Resolution;
    public GameObject EmployeeOfTheMonthObj;
}
