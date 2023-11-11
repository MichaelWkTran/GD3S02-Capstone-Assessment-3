using System;
using UnityEngine;

static class InputUtilities
{
    public static bool GetJoystickButton(int _slot, int _buttonIndex)
    {
        return Input.GetKey
        (
            (KeyCode)Enum.Parse(typeof(KeyCode),
            "Joystick" + (_slot + 1) + "Button" + _buttonIndex)
        );
    }

    public static bool GetJoystickButtonDown(int _slot, int _buttonIndex)
    {
        return Input.GetKeyDown
        (
            (KeyCode)Enum.Parse(typeof(KeyCode),
            "Joystick" + (_slot + 1) + "Button" + _buttonIndex)
        );
    }

    public static bool GetJoystickButtonUp(int _slot, int _buttonIndex)
    {
        return Input.GetKeyUp
        (
            (KeyCode)Enum.Parse(typeof(KeyCode),
            "Joystick" + (_slot + 1) + "Button" + _buttonIndex)
        );
    }
}
