using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testCarNUI : MonoBehaviour
{
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    public Rigidbody car;
    public float maxSpeed;

    private void Move()
    {
        float currentSpeed = car.velocity.magnitude * 3.6f;
        //int maxspeed = maxSpeed;

        if ((int)currentSpeed >= maxSpeed)
        {
            wheelRL.motorTorque = -100;
            wheelRR.motorTorque = -100;
        }
        else
        {
            wheelRL.motorTorque = 550;
            wheelRR.motorTorque = 550;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        car.constraints = RigidbodyConstraints.FreezeRotationY;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
