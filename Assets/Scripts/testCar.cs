using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class testCar : MonoBehaviour
{
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    public Rigidbody car;
    public Text speed;
    public float maxSpeed;
    public InputField speedInput; //輸入框

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

        speed.text = "前方車速: " + ((int)currentSpeed) + " Km/h";
        speed.color = Color.yellow;
    }


    // Start is called before the first frame update
    void Start()
    {
        car.constraints = RigidbodyConstraints.FreezeRotationY; //維持車道中間

        //speedInput.text = carController.speed.ToString(); // 初始化輸入框內
        
        speedInput.onEndEdit.AddListener(OnInputFieldValueChanged); // 當輸入框內容改變時執行
    }

    void OnInputFieldValueChanged(string value)
    {
        if (float.TryParse(value, out float newSpeed))
        {
            maxSpeed = newSpeed; // 更新車速
        }
        else
        {
            Debug.LogWarning("無效的速度輸入");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
