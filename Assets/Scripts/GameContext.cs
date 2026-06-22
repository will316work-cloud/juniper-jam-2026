[System.Serializable]
public class GameContext
{
    public TaskManager TaskManager;
    public MoneyController MoneyController;
    public GameInput GameInput;
    public PlayerControl PlayerControl;
    public UiManager UiManager;
    public GameStateController GameStateController;
    public PlayerInteractor PlayerInteractor;
    public WorldHealthMeter WorldHealthMeter;
    public DayTimeController DayTimeController;
    public CoworkerManager CoworkerManager;
    public BatteryDropoff BatteryDropoff;
    public Battery Battery;
    public TransitionController TransitionController;
    public AudioPool AudioPool;
}