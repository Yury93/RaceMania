using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarChassis))]
public class Car : MonoBehaviour
{
    private CarChassis carChassis;
    [SerializeField] private float maxBreakTorque, maxSteerAngle;
   
    [SerializeField] private AnimationCurve animationEngineCurve;
    [SerializeField] private float maxMotorTorque;
    [SerializeField] private float maxSpeed;

    [Header("Dynamically value")]
    [SerializeField] private float motorTorqueControll, breakControl, steerControll;

    public float LinearVelocity => carChassis.LeanerVelocity;

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

    private void Update()
    {
        float engineTorque = animationEngineCurve.Evaluate(LinearVelocity / maxSpeed) * maxMotorTorque;

        if (LinearVelocity >= maxSpeed) 
        {
            engineTorque = 0; 
        }

        carChassis.MotorTorque = engineTorque * motorTorqueControll;
        carChassis.BreakTorque = breakControl* maxBreakTorque;
        carChassis.SteerAngle = steerControll * maxSteerAngle;
    }
  
}
