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
    public InputField speedInput; //��J��

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

        speed.text = "�e�訮�t: " + ((int)currentSpeed) + " Km/h";
        speed.color = Color.yellow;
    }


    // Start is called before the first frame update
    void Start()
    {
        car.constraints = RigidbodyConstraints.FreezeRotationY; //�������D����

        //speedInput.text = carController.speed.ToString(); // ��l�ƿ�J�ؤ�
        
        speedInput.onEndEdit.AddListener(OnInputFieldValueChanged); // ���J�ؤ��e���ܮɰ���
    }

    void OnInputFieldValueChanged(string value)
    {
        if (float.TryParse(value, out float newSpeed))
        {
            maxSpeed = newSpeed; // ��s���t
        }
        else
        {
            Debug.LogWarning("�L�Ī��t�׿�J");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
