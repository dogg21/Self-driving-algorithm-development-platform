using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        //�I�s�������
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        //���}
        Application.Quit();
    }
}
