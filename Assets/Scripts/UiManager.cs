using OpenCvSharp.ImgHash;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    private SensorNUI Sensor;

    //car
    public GameObject car;

    //speedometer
    private float minSpeedArrowAngle = 10.309f;
    private float maxSpeedArrowAngle = -188.546f;
    public RectTransform arrow;

    //img物件
    private Image leftblindImg;
    private Image rightblindImg;

    //text物件
    private Text carspeed;
    private Text distance;
    private Text leftblind;
    private Text rightblind;

    void Start()
    {
        if (carspeed == null)
        {
            GameObject carspeedObject = GameObject.Find("Carspeed");

            if (carspeedObject != null)
            {
                carspeed = carspeedObject.GetComponent<Text>();
            }
        }

        if (distance == null)
        {
            GameObject distanceObject = GameObject.Find("Distance");

            if (distanceObject != null)
            {
                distance = distanceObject.GetComponent<Text>();
            }

        }

        if (leftblind == null)
        {
            GameObject leftblindObject = GameObject.Find("leftblind");

            if (leftblindObject != null)
            {
                GameObject leftIconObject = leftblindObject.transform.Find("img").gameObject;
                if (leftIconObject != null)
                {
                    leftblindImg = leftIconObject.GetComponent<Image>();
                    leftblindImg.enabled = false; // 假設這裡是關閉圖示的可見性
                    //Debug.LogWarning("rightblindImg is exist.");
                }
                leftblind = leftblindObject.GetComponent<Text>();
                leftblind.color = Color.white;
            }
        }

        if (rightblind == null)
        {
            GameObject rightblindObject = GameObject.Find("rightblind");

            if (rightblindObject != null)
            {
                GameObject rightIconObject = rightblindObject.transform.Find("img").gameObject;
                if (rightIconObject != null)
                {
                    rightblindImg = rightIconObject.GetComponent<Image>();
                    rightblindImg.enabled = false; // 假設這裡是關閉圖示的可見性
                    //Debug.LogWarning("rightblindImg is exist.");
                }
                rightblind = rightblindObject.GetComponent<Text>();
                rightblind.color = Color.white;
            }
        }

        if (arrow == null)
        {
            GameObject arrowObject = GameObject.Find("speedometer");

            if (arrowObject != null)
            {
                Transform arrowTransform = arrowObject.transform.Find("Arrow");

                if (arrowTransform != null)
                {
                    arrow = arrowTransform.GetComponent<RectTransform>();
                }
            }

            if (car != null)
            {
                Sensor = car.GetComponent<SensorNUI>();

            }
        }
    }

    void Update()
    {
        float cspeed = Sensor.carSpeedMeters();
        float dis = Sensor.currentDistance;

        if (Sensor != null)
        {

            carspeed.text = ((int)cspeed) + " Km/h";
            carspeed.color = Color.white;

            if (arrow != null)
            {
                float speedPercent = Mathf.Clamp(cspeed / 260f, 0f, 1f);  // 假設最大速度為 260 km/h
                //float arrowAngle = Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, cspeed / 260);
                arrow.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speedPercent));
            }

            if (Sensor.accBool() != false)
            {
                distance.text = "與前車距離: " + ((int)dis) + " m";
                distance.color = Color.yellow;
            }
            else
            {
                distance.text = "未偵測到車輛";
                distance.color = Color.yellow;
            }

            if (Sensor.leftblindbool() != false)
            {
                leftblindImg.enabled = true;
            }
            else
            {
                leftblindImg.enabled = false;
            }

            if (Sensor.rightblindbool() != false)
            {
                rightblindImg.enabled = true;
            }
            else
            {
                rightblindImg.enabled = false;
            }

        }
        else
        {
            distance.text = "未偵測到車輛";
            distance.color = Color.yellow;
        }
    }

}
