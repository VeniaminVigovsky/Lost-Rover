using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverMovement : MonoBehaviour
{
    Rover rover;


    float GetRoverAngle()
    {
        return rover.transform.rotation.z;
    }
}
