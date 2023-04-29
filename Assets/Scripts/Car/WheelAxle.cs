using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WheelAxle 
{
    [SerializeField]private  WheelCollider LeftWheelCollider;
    [SerializeField] private WheelCollider RightWheelCollider;
    [SerializeField] private Transform LeftWheelTransform;
    [SerializeField] private Transform RightWheelTransform;
    [SerializeField] private bool isSteer;
    [SerializeField] private bool isMotor;
    [SerializeField] private float AntiRollForce;
    [SerializeField] private float motorTorque;
    [SerializeField] private float widthWheelAxle;
    [SerializeField] private float baseForwardStiffness = 1.5f, stabilityForwardFactor = 1.0f;
    [SerializeField] private float baseSideStiffness = 2f, stabilitySideFactor = 1.0f;
    [SerializeField] private float additionalWheelDownForce;
    WheelHit leftGroundHit,rightGroundHit;
    public bool IsMotor => isMotor;
    public bool IsSteer => isSteer;
    public void Update()
    {
        UpdateGroundHit();

        ApplyAntiRoll();
        ApplyDownForce();
        CorrectStiffness();

        SyncMeshTransform();
    }

    /// <summary>
    /// Жесткость
    /// </summary>
    private void CorrectStiffness()
    {
        WheelFrictionCurve rightForward = RightWheelCollider.forwardFriction;
        WheelFrictionCurve leftForward= LeftWheelCollider.forwardFriction;

        WheelFrictionCurve rightSide = RightWheelCollider.forwardFriction;
        WheelFrictionCurve leftSide = LeftWheelCollider.forwardFriction;

        leftForward.stiffness = baseForwardStiffness * Mathf.Abs(leftGroundHit.forwardSlip) * stabilityForwardFactor;
        rightForward.stiffness = baseForwardStiffness * Mathf.Abs(rightGroundHit.forwardSlip) * stabilityForwardFactor;
       leftSide.stiffness = baseSideStiffness * Mathf.Abs(leftGroundHit.sidewaysSlip) * stabilitySideFactor;
        rightSide.stiffness = baseSideStiffness * Mathf.Abs(rightGroundHit.sidewaysSlip) * stabilitySideFactor;

        LeftWheelCollider.sidewaysFriction = leftForward;
        RightWheelCollider.sidewaysFriction = rightForward;

        LeftWheelCollider.sidewaysFriction = leftSide;
        RightWheelCollider.sidewaysFriction = rightSide;
    }

    internal void ApplySteerAngle(float steerAngle, object wheelBaseLenght)
    {

    }

    private void ApplyDownForce()
    {
        if(LeftWheelCollider.isGrounded == true)
        {
            LeftWheelCollider.attachedRigidbody
                .AddForceAtPosition(leftGroundHit.normal * -additionalWheelDownForce 
                * LeftWheelCollider.attachedRigidbody.velocity.magnitude, LeftWheelCollider.transform.position);
        }
        if (RightWheelCollider.isGrounded == true)
        {
            RightWheelCollider.attachedRigidbody
                .AddForceAtPosition(rightGroundHit.normal * -additionalWheelDownForce
                * RightWheelCollider.attachedRigidbody.velocity.magnitude, RightWheelCollider.transform.position);
        }
    }

    private void ApplyAntiRoll()
    {
        float travelL = 1.0F;
            float travelR = 1.0F; 
        if(LeftWheelCollider.isGrounded == true)
        {
          travelL =  (-LeftWheelCollider.transform.InverseTransformPoint(leftGroundHit.point).y - LeftWheelCollider.radius)/ LeftWheelCollider.suspensionDistance;
        }
        if (RightWheelCollider.isGrounded == true)
        {
            travelR = (-RightWheelCollider.transform.InverseTransformPoint(rightGroundHit.point).y - RightWheelCollider.radius) / RightWheelCollider.suspensionDistance;
        }
        var forceDir = travelL - travelR;
       
        if (LeftWheelCollider.isGrounded == true)
        {
            LeftWheelCollider.attachedRigidbody.AddForceAtPosition(LeftWheelCollider.transform.up * -forceDir * AntiRollForce, LeftWheelCollider.transform.position);
        }
        if (RightWheelCollider.isGrounded == true)
        {
            RightWheelCollider.attachedRigidbody.AddForceAtPosition(RightWheelCollider.transform.up * forceDir * AntiRollForce, RightWheelCollider.transform.position);
        }
      

        
    }

    private void UpdateGroundHit()
    {
        LeftWheelCollider.GetGroundHit(out leftGroundHit);
        RightWheelCollider.GetGroundHit(out rightGroundHit);
    }

    public void ApplySteerAngle(float steerAngle, float wheelBaseLenght)
    {
        if (!IsSteer) return;

        var radius = Mathf.Abs(wheelBaseLenght * Mathf.Tan(Mathf.Deg2Rad * ( 90f - Mathf.Abs(steerAngle))));
        var angleSing = Mathf.Sign(steerAngle);
        Debug.Log(angleSing + " // -1 0 1");
        if(steerAngle > 0)
        {
            LeftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLenght/( radius+ (widthWheelAxle * 0.5f))) * angleSing ;
            RightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLenght / (radius - (widthWheelAxle * 0.5f))) * angleSing;
        }
        else if(steerAngle < 0)
        {
            LeftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLenght / (radius - (widthWheelAxle * 0.5f))) * angleSing;
           RightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLenght / (radius + (widthWheelAxle * 0.5f))) * angleSing;
        }
        else
        {
            LeftWheelCollider.steerAngle = 0;
            RightWheelCollider.steerAngle = 0;
        }

      
    }
    public void ApplyMotorTorque(float motorTorque)
    {
        if (!IsMotor) return;

        LeftWheelCollider.motorTorque = motorTorque;
        RightWheelCollider.motorTorque = motorTorque;
        this.motorTorque = motorTorque;
    }
    public void ApplyBreakTorque(float breakTorque)
    {
        LeftWheelCollider.brakeTorque = breakTorque;
        RightWheelCollider.brakeTorque = breakTorque;
    }

    private void SyncMeshTransform()
    {
        UpdateWheelTransform(LeftWheelCollider,LeftWheelTransform);
        UpdateWheelTransform(RightWheelCollider, RightWheelTransform);
    }
    private void UpdateWheelTransform(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 wheelPosition;
        Quaternion wheelRotation;

        wheelCollider.GetWorldPose(out wheelPosition, out wheelRotation);
        wheelTransform.position = wheelPosition;
        wheelTransform.rotation = wheelRotation;
    }
}
