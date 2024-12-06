using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SMeneger1 : MonoBehaviour
{
    // Start is called before the first frame update
    public void Next()
    {
        SceneManager.LoadScene(2);
    }

    // Update is called once per frame
    public void Previous()
    {
        //Â÷¶}
        SceneManager.LoadScene(0);
    }
}
