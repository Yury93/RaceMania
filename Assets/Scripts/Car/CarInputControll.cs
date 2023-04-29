using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputControll : MonoBehaviour
{
    [SerializeField] private Car car;
    void Update()
    {
        car.SetMotorTorque(Input.GetAxis("Vertical"));
        car.SetBreakTorque(Input.GetAxis("Jump"));
        car.SetSteerAngle(Input.GetAxis("Horizontal"));
    }
}
