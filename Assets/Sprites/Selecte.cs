using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;

public class Selecte : MonoBehaviour
{
    public Button next;
    public Button prev;
    private int currentCar;
    public GameObject[] cars;
    int index;
    public float Map;
    // Start is called before the first frame update
    void Start()
    {
        index = PlayerPrefs.GetInt("carIndex");

        for(int i=0;i<cars.Length;i++)
        {
            cars[i].SetActive(false);
            cars[index].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(index>=2)
        {
            next.interactable = false;
        }
        else
        {
            next.interactable = true;
        }
        if (index <= 0)
        {
            prev.interactable = false;
        }
        else
        {
            prev.interactable = true;
        }
    }
    public void Next()
    {
        index++;
        for(int i =0;i<cars.Length;i++)
        {
            cars[i].SetActive(false);
            cars[index].SetActive(true);
        }
        PlayerPrefs.SetInt("carIndex", index);
        PlayerPrefs.Save();
    }
    public void Prev()
    {
        index--;
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].SetActive(false);
            cars[index].SetActive(true);
        }
        PlayerPrefs.SetInt("carIndex", index);
        PlayerPrefs.Save();
    }
    public void Race()
    {
        SceneManager.LoadScene("Map" + Map.ToString());
    }
}
