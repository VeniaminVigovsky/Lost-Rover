using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverAnimationEventsHandler : MonoBehaviour
{
    Rover rover;
    private void Awake()
    {
        rover = GetComponentInParent<Rover>();
    }

    public void OnFallAnimationEnded()
    {
        rover.RoverFallen();
    }
}
