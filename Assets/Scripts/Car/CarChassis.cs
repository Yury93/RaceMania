using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class CarChassis : MonoBehaviour
{
    [SerializeField] private WheelAxle[] wheelAxles;
   [SerializeField] private float wheelBaseLenght;
    [SerializeField] private Transform centrMass;
    [Header("DownForce")]
    [SerializeField] private float downForceDragMin, downForceDragMax, downForceDragFactor;
    [Header("AngularDrag")]
    [SerializeField] private float angularDragMin, angularDragMax, angularDragFactor;

    public float MotorTorque;
    public float SteerAngle;
    public float BreakTorque;
    private new Rigidbody rigidbody;
    public float LeanerVelocity => rigidbody.velocity.magnitude * 3.6f; // ������� �� 3.6 ����� ��������� �/� � ��/�
    public float WheelSpeed => GetWheelSpeed();
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        if(centrMass!= null)
        {
            rigidbody.centerOfMass = centrMass.localPosition;
        }
    }

    private void FixedUpdate()
    {
        UpdateAngularDrag();
        UpdateDownForce();
        UpdateWheelAxles();
    }


    private float GetAvarageRpm()
    {
        float sum = 0;
        for (int i = 0; i < wheelAxles.Length; i++)
        {
            sum += wheelAxles[i].GetAvarageRpm();
        }
        return sum / wheelAxles.Length;
    }
    public float GetWheelSpeed()
    {
        return GetAvarageRpm() * wheelAxles[0].GetRadius() * 2 * 0.1885f;
    }



    private void UpdateAngularDrag()
    {
        rigidbody.angularDrag = Mathf.Clamp(angularDragFactor * LeanerVelocity, angularDragMin, angularDragMax);
    }

    private void UpdateDownForce()
    {
        float downForce = Mathf.Clamp(downForceDragFactor * LeanerVelocity, downForceDragMin, downForceDragMax);
        rigidbody.AddForce(-transform.up * downForce);
    }

    private void UpdateWheelAxles()
    {
        int amountMotorWheel = 0;

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            if(wheelAxles[i].IsMotor)
            {
                amountMotorWheel += 2;
            }
        }


        for (int i = 0; i < wheelAxles.Length; i++)
        {
            wheelAxles[i].Update();
            wheelAxles[i].ApplyMotorTorque(MotorTorque/amountMotorWheel);
            wheelAxles[i].ApplyBreakTorque(BreakTorque);
            wheelAxles[i].ApplySteerAngle(SteerAngle, wheelBaseLenght);
        }
    }
}
