using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarChassis : MonoBehaviour
{
    [SerializeField] private WheelAxle[] wheelAxles;
   [SerializeField] private float wheelBaseLenght;
    public float MotorTorque;
    public float SteerAngle;
    public float BreakTorque;


    private void Update()
    {
        UpdateWheelAxles();
    }
    private void UpdateWheelAxles()
    {
        for (int i = 0; i < wheelAxles.Length; i++)
        {
            wheelAxles[i].Update();
            wheelAxles[i].ApplyMotorTorque(MotorTorque);
            wheelAxles[i].ApplyBreakTorque(BreakTorque);
            wheelAxles[i].ApplySteerAngle(SteerAngle, wheelBaseLenght);
        }
    }
}
