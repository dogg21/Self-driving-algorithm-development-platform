using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimation : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 finalPosition;
    // Start is called before the first frame update
    private void Awake()
    {
        initialPosition = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, finalPosition, 0.1f);
    }

    private void OnDisable() 
    {
        transform.position = initialPosition;
    }
}
