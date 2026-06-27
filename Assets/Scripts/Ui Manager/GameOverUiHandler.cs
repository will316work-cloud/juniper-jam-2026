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
    Button _sendScoreButton;
    GameContext _ctx;
    TMP_InputField _inputField;

    public void Initialize(GameContext ctx, GameOverUiHandlerData data)
    {
        _ctx = ctx;

        _panel = data.Panel;
        _scoreText = data.ScoreText;
        _restartButton = data.RestartButton;
        _mainMenuButton = data.MainMenuButton;
        _lossReasonText = data.LossReasonText;
        _sendScoreButton = data.SendScoreButton;
        _inputField = data.InputField;

        _restartButton.onClick.AddListener(() => GameOverButtonClickHandler());
        _mainMenuButton.onClick.AddListener(() => _ctx.GameStateController.ChangeState(StateType.MainMenu));
        _sendScoreButton.onClick.AddListener(() => HandleSendScore());

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

        _sendScoreButton.interactable = true;
        _inputField.text = "";
    }

    void GameOverButtonClickHandler()
    {
        _ctx.PoolManager.GetSfx(AudioType.UiClick);
        _ctx.GameStateController.ChangeState(StateType.Gameplay);
    }

    void HandleSendScore()
    {
        if(_inputField.text == "" || _inputField.text.Length < 3)
        {
            _inputField.text = "At least 3 characters!";
            return;
        }
        _ctx.UiManager.ScoreBoardController.PostScore(_inputField.text, _ctx.MoneyController.CurrentMoney());
        _sendScoreButton.interactable = false;
        _inputField.text = "Score Sent!";
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
    public Button SendScoreButton;
    public TMP_InputField InputField;

}