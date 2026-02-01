using UnityEngine;

public class GameInput : MonoBehaviour
{
    private InputActions inputActions;
    public static GameInput Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        inputActions = new InputActions();
        inputActions.Player.Enable();

    }

    public bool IsUpActionPressed()
    {
        return inputActions.Player.PlayerUp.IsPressed();
    }

    public bool IsDownActionPressed()
    {
        return inputActions.Player.PlayerDown.IsPressed();
    }

    public bool IsLeftActionPressed()
    {
        return inputActions.Player.PlayerLeft.IsPressed();
    }

    public bool IsRightActionPressed()
    {
        return inputActions.Player.PlayerRight.IsPressed();
    }

    private void OnDestroy()
    {
        inputActions.Player.Disable();
    }
    public bool IsInteractPressed()
    {
        return inputActions.Player.PlayerInterract.WasPressedThisFrame();
    }

}
