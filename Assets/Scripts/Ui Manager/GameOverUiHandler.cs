using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUiHandler
{
    GameObject _panel;
    TextMeshProUGUI _scoreText;
    TextMeshProUGUI _lossReasonText;
    Button _restartButton;
    Button _mainMenuButton;
    GameContext _ctx;

    public void Initialize(GameContext ctx, GameOverUiHandlerData data)
    {
        _ctx = ctx;

        _panel = data.Panel;
        _scoreText = data.ScoreText;
        _restartButton = data.RestartButton;
        _mainMenuButton = data.MainMenuButton;
        _lossReasonText = data.LossReasonText;

        _restartButton.onClick.AddListener(() => GameOverButtonClickHandler());
        _mainMenuButton.onClick.AddListener(() => _ctx.GameStateController.ChangeState(StateType.MainMenu));

        SetPanelState(false);
    }

    public bool IsPanelActive() => _panel.activeSelf;
    public void SetPanelState(bool state, LooseReason reason = LooseReason.None)
    {
        if(_ctx.GameStateController.LooseReason is LooseReason.None) _lossReasonText.text = "";
        if(_ctx.GameStateController.LooseReason is LooseReason.Quota) _lossReasonText.text = "You haven't met your quota!";
        if(_ctx.GameStateController.LooseReason is LooseReason.Health) _lossReasonText.text = "The world has run out of energy!";

        _scoreText.text = $"You earned: {_ctx.MoneyController.CurrentMoney()}$";
        _panel.SetActive(state);
    }

    void GameOverButtonClickHandler()
    {
        _ctx.PoolManager.GetSfx(AudioType.UiClick);
        _ctx.GameStateController.ChangeState(StateType.Gameplay);
    }
}
 
[System.Serializable]
public class GameOverUiHandlerData
{
    public GameObject Panel;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI LossReasonText;
    public Button RestartButton;
    public Button MainMenuButton;
}