using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void ExitGame()
    {
        Application.Quit();
    }

 
    public void EnterManagement()
    {
        SceneManager.LoadScene(1);
    }
}
