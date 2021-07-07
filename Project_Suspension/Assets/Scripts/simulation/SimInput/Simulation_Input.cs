using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Simulation_Input : ScriptableObject
{
    public float motor_force; 

    [SerializeField]
    private float steer_angle;// in deg
    [SerializeField]
    private float acc_force, brake_force;// in N
    public float GetSteerInput()
    {
        if (Input.GetKey("d"))
            steer_angle = steer_angle + 1f;
        if (Input.GetKey("a"))
            steer_angle = steer_angle - 1f;
        steer_angle = Mathf.Clamp(steer_angle, -180, 180);
        return steer_angle;
    }// return steering wheel angle, in deg

    public float GetAccelarationInput()
    {
        if (Input.GetKey("w"))
            acc_force = motor_force;
        else
            acc_force = 0f;
        return acc_force;
    }// return motor force, in N

    public float GetBrakeInput()
    {
        return brake_force;
    }// return breaking force, in N (to be inplemented)
}
