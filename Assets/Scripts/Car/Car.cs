using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarChassis))]
public class Car : MonoBehaviour
{
    private CarChassis carChassis;
    [SerializeField] private float maxMotorTorque,  maxBreakTorque, maxSteerAngle;
    [SerializeField] private float motorTorqueControll, breakControl, steerControll;

    private void Start()
    {
        Debug.Log("Start");
        carChassis = GetComponent<CarChassis>();
    }
    public void SetMotorTorque(float motorTorque)
    {
        motorTorqueControll = motorTorque;
    }
    public void SetBreakTorque(float breakTorque)
    {
        breakControl = breakTorque;
    }
    public void SetSteerAngle(float steerAngle)
    {
        this.steerControll = steerAngle;
    }

    private void FixedUpdate()
    {
        carChassis.MotorTorque = motorTorqueControll * maxMotorTorque;
        carChassis.BreakTorque = breakControl* maxBreakTorque;
        carChassis.SteerAngle = steerControll * maxSteerAngle;
    }
  
}
