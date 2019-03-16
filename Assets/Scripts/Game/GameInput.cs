using System.Collections.Generic;
using UnityEngine;

namespace UnityMMO{
public class GameInput
{
    static GameInput instance;
    Dictionary<KeyCode, bool> keyUp;
    Vector2 joystickDir;
    private object InputSystem;

    public Vector2 JoystickDir 
    { 
        get {
            if (joystickDir.sqrMagnitude>0)
            {
                return joystickDir; 
            }
            else
            {
                Vector2 input = new Vector2();
                input.x = Input.GetAxis("Horizontal");
                input.y = Input.GetAxis("Vertical");
                return input;
            }
        }
        set => joystickDir = value; 
    }

    public static GameInput GetInstance()
    {
        if (instance!=null)
            return instance;
        instance = new GameInput();
        return instance;
    }

    public void Reset()
    {
        foreach(var keyInfo in keyUp)
        {
            keyUp[keyInfo.Key] = false;
        }
        // JoystickDir = Vector2.zero;
    }

    public bool GetKeyUp(KeyCode key)
    {
        bool isUp = false;
        keyUp.TryGetValue(key, out isUp);
        return isUp || UnityEngine.Input.GetKeyUp(key);
    }

    public void SetKeyUp(KeyCode key, bool isUp=true)
    {
        keyUp[key] = isUp;
    }

    private GameInput()
    {
        keyUp = new Dictionary<KeyCode, bool>();
    }
}

}