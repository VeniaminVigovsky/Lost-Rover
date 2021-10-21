using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager 
{
    static bool isInputLocked = false;

    static bool isMakingPath = false;

    static Rover rover;

    static float timeToPeak = 1f;

    public static void RegisterRover(Rover _rover)
    {
        rover = _rover;
    }

    public static Rover GetRover()
    {
        return rover;
    }

    public static void LockInput()
    {
        isInputLocked = true;
    }

    public static void UnlockInput()
    {
        isInputLocked = false;
    }

    public static bool InputLocked()
    {
        return isInputLocked;
    }

    public static void SetMakingPath(bool value)
    {
        isMakingPath = value;
    }

    public static bool IsMakingPath()
    {
        return isMakingPath;
    }

    public static float GetTimeToPeak()
    {
        return timeToPeak;
    }

    public static void SetTimeToPeak(float newTimeToPeak)
    {
        timeToPeak = newTimeToPeak;
    }
}
