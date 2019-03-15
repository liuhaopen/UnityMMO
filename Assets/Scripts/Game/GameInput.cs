using System.Collections.Generic;
using UnityEngine;

namespace UnityMMO{
public class GameInput
{
    static GameInput instance;
    Dictionary<KeyCode, bool> keyUp;
        private object InputSystem;

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