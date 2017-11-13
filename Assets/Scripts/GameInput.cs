using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput {

    private static bool isBlocked = false;

    public static bool IsTriggered()
    {
        var isDown = IsDown();

        if (isBlocked && !isDown) 
        {
            isBlocked = false;
        }
        else if (isDown && !isBlocked)
        {
            isBlocked = true;
            return true;
        }

        return false;
    }

    public static bool IsDown()
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
            return true;
        else
            return false;
    }
}