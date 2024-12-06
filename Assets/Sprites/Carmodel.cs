using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carmodel : MonoBehaviour
{
    [SerializeField] private GameObject[] carModels;

    private void Awake()
    {
        ChooseCarModel(SaveManager.instance.currentCar);
    }
    private void ChooseCarModel(int _index) 
    {
        Instantiate(carModels[_index], transform.position, Quaternion.identity, transform);
    }
}
