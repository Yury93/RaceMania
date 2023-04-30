using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputControll : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private AnimationCurve breakCurve,steerCurve;
    [Range(0, 1)]
    [SerializeField] private float autoBreakStrength = 0.5f;
    private float wheelSpeed, verticalAxis, horizontalAxis, handBreakAxis;

    
    void Update()
    {
        wheelSpeed = car.WheelSpeed;

        UpdateAxis();

        UpdateThrottleAndBreak();
        UpdateSteerAngle();

        UpdateAutoBreak();
    }

    private void UpdateAxis()
    {
        verticalAxis = Input.GetAxis("Vertical");
        horizontalAxis = Input.GetAxis("Horizontal");
        handBreakAxis = Input.GetAxis("Jump");
    }

    private void UpdateSteerAngle()
    {
      
        float steer = steerCurve.Evaluate(wheelSpeed / car.MaxSpeed) * horizontalAxis ;
        car.SetSteerAngleControll(steer);
    }

    private void UpdateThrottleAndBreak()
    {
        if (Mathf.Sign(verticalAxis) == Mathf.Sign(wheelSpeed) || Mathf.Sign(wheelSpeed) < 0.5f)
        {
            car.SetMotorTorqueControll(verticalAxis);
            car.SetBreakTorqueControll(0);
        }
        else
        {
            car.SetMotorTorqueControll(0);
            var breakControll = breakCurve.Evaluate(wheelSpeed / car.MaxSpeed);
            car.SetBreakTorqueControll(breakControll);
        }
    }

    private void UpdateAutoBreak()
    {
        if (verticalAxis == 0)
        {
            var breakControll = breakCurve.Evaluate(wheelSpeed / car.MaxSpeed) * autoBreakStrength;
            car.SetBreakTorqueControll(breakControll);
        }
    }
}
