using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        //©I¥s¨®½ø¿ï¾Ü
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        //Â÷¶}
        Application.Quit();
    }
}
