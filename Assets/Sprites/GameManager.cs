using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int index;
    public GameObject[] cars;
    public Text carspeed;

    void Start()
    {
        index = PlayerPrefs.GetInt("carIndex");
        GameObject car = Instantiate(cars[index],Vector3.zero,Quaternion.identity);
    }
}
