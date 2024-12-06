using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Seletor : MonoBehaviour
{
    public int MapCondition;
    // Start is called before the first frame update

    // Update is called once per frame
    public void OpenScene()
    {
        SceneManager.LoadScene("MapCondition" + MapCondition.ToString());
    }
}
