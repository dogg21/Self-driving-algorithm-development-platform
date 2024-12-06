using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testCarSlow : MonoBehaviour
{
    // Start is called before the first frame update
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    public Rigidbody car;
    public Text speed;
    public int maxSpeed;

    private void Move()
    {
        float currentSpeed = car.velocity.magnitude * 3.6f;
        //int maxspeed = maxSpeed;

        if ((int)currentSpeed >= maxSpeed)
        {
            car.drag = Mathf.Lerp(car.drag, 0.5f, Time.deltaTime);
        }
        else
        {
            wheelRL.motorTorque = 550;
            wheelRR.motorTorque = 550;
        }

        speed.text = "前方車速:" + ((int)currentSpeed) + "Km/h";
        speed.color = Color.yellow;
    }


    // Start is called before the first frame update
    void Start()
    {
        car.constraints = RigidbodyConstraints.FreezeRotationY; //維持車道中間
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
