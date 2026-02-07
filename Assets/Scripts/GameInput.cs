using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private InputActions inputActions;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        inputActions = new InputActions();
        inputActions.Player.Enable();
    }

    private void OnEnable()
    {
        inputActions?.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions?.Player.Disable();
    }

    public bool IsUpActionPressed() => inputActions.Player.PlayerUp.IsPressed();
    public bool IsDownActionPressed() => inputActions.Player.PlayerDown.IsPressed();
    public bool IsLeftActionPressed() => inputActions.Player.PlayerLeft.IsPressed();
    public bool IsRightActionPressed() => inputActions.Player.PlayerRight.IsPressed();

    public bool IsInteractPressed() =>
        inputActions.Player.PlayerInterract.WasPressedThisFrame();

    public bool IsPauseActionPressed() =>
        inputActions.Player.PlayerPause.triggered;
}
