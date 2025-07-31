using Fusion;

public struct MyNetworkInput : INetworkInput
{
    public const int BUTTON_FORWARD = 0;
    public const int BUTTON_BACKWARD = 1;
    public const int BUTTON_LEFT = 2;
    public const int BUTTON_RIGHT = 3;
    public const int BUTTON_JUMP = 4;
    public const int BUTTON_ACTION1 = 5;
    public const int BUTTON_FIRE = 6;

    public NetworkButtons Buttons;

    public bool IsUp(int button)
    {
        return Buttons.IsSet(button) == false;
    }

    public bool IsDown(int button)
    {
        return Buttons.IsSet(button);
    }
}