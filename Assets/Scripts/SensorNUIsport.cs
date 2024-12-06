using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SensorNUIsport : MonoBehaviour
{
    //car
    public Vector3 centerOfMass;
    public float AccDistance;
    //public float avoidspeed = 10;
    //private bool avoiding = false;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    //car speed
    public Rigidbody car;
    public float frontcars;
    public float maxSpeed = 5.0f;
    public float minSpeed = 0.0f;

    //sensor
    public float sensorLength = 5f;
    public float backsensorLength = 5f;
    public Vector3 frontSensorPosition = new(0f, 0.2f, 0.5f);
    public float frontSideSensorPosition = 0.2f;
    public float frontSensorAngle = 30;

    //back sensor
    public Vector3 backSensorPosition = new(0f, 0.2f, 0.5f);
    public float backSideSensorPosition = 0.2f;
    public float backSensorAngle = 30;

    //message



    //car acc
    private float differD;
    private float previousDistance = 0f;
    public float currentDistance = 0f;
    private float TimerCount = 0f;


    void Start()
    {
        car.constraints = RigidbodyConstraints.FreezeRotationY;
    }

    public float carSpeedMeters()
    {
        float carspeed = car.velocity.magnitude * 3.6f;
        return carspeed;
    }

    private float subDistance(float currentDistance)
    {
        TimerCount += Time.deltaTime;
        if (TimerCount >= 1f)
        {
            differD = currentDistance - previousDistance;
            previousDistance = currentDistance;
            TimerCount = 0f;
        }
        return differD;
    }

    private void CalcalculateSpeed(float targetSpeed)
    {
        float targetSpeedMeters = targetSpeed / 3.6f; // 将目标速度转换为 m/s
        float carspeed = car.velocity.magnitude;
        float speedDifference = targetSpeedMeters - carspeed;
        float motorTorque = Mathf.Clamp(speedDifference * 550f, -550f, 550f);
        wheelRL.motorTorque = motorTorque;
        wheelRR.motorTorque = motorTorque;
    }

    public bool accBool()
    {
        RaycastHit hit;
        Vector3 sensorStarPos = transform.position + frontSensorPosition;
        if (Physics.Raycast(sensorStarPos, transform.forward, out hit, sensorLength))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool leftblindbool()
    {
        RaycastHit hit;
        Vector3 backsensorStarPos = transform.position + backSensorPosition;
        backsensorStarPos.x -= 2 * backSideSensorPosition;

        if (Physics.Raycast(backsensorStarPos, Quaternion.AngleAxis(-backSensorAngle, transform.up) * transform.forward, out hit, backsensorLength))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool rightblindbool()
    {
        RaycastHit hit;
        Vector3 backsensorStarPos = transform.position + backSensorPosition;
        backsensorStarPos.x += backSideSensorPosition;

        if (Physics.Raycast(backsensorStarPos, Quaternion.AngleAxis(backSensorAngle, transform.up) * transform.forward, out hit, backsensorLength))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Sensors()
    {

        RaycastHit hit;
        Vector3 sensorStarPos = transform.position + frontSensorPosition;
        Vector3 backsensorStarPos = transform.position + backSensorPosition;
        //float avoidMultiplier = 0;
        //avoiding = false;

        //center
        if (Physics.Raycast(sensorStarPos, transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorStarPos, hit.point);
            currentDistance = hit.distance;

            float frontcar = subDistance(currentDistance);
            frontcars = carSpeedMeters() + (frontcar / 1f * 3.6f);

            if (currentDistance > AccDistance)
            {
                CalcalculateSpeed(120);
            }
            else
            {
                CalcalculateSpeed(Mathf.Min(120, frontcars + 10 * ((currentDistance - AccDistance) / (AccDistance + 10 - AccDistance))));
            }

        }
        else
        {
            CalcalculateSpeed(120);
        }

        //right
        sensorStarPos.x += frontSideSensorPosition;
        if (Physics.Raycast(sensorStarPos, transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorStarPos, hit.point);
        }

        //left
        sensorStarPos.x -= 2 * frontSideSensorPosition;
        if (Physics.Raycast(sensorStarPos, transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorStarPos, hit.point);
        }

        //back right
        backsensorStarPos.x += backSideSensorPosition;
        if (Physics.Raycast(backsensorStarPos, Quaternion.AngleAxis(backSensorAngle, transform.up) * transform.forward, out hit, backsensorLength))
        {
            Debug.DrawLine(backsensorStarPos, hit.point, Color.red);
        }

        //back left
        backsensorStarPos.x -= 2 * backSideSensorPosition;
        if (Physics.Raycast(backsensorStarPos, Quaternion.AngleAxis(-backSensorAngle, transform.up) * transform.forward, out hit, backsensorLength))
        {
            Debug.DrawLine(backsensorStarPos, hit.point, Color.red);
        }
    }


    /*private void GetSteer()
    {
        if (avoiding) return;
    }*/

    private void Move()
    {
        float currentSpeed = car.velocity.magnitude * 3.6f;

    }
    void FixedUpdate()
    {
        Sensors();
        Move();

        //carSpeedMeters();
        //GetSteer();
    }
}
