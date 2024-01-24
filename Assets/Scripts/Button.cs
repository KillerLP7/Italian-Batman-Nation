using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void Back()
    {
        Options.OnExit();
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        Options.OnExit();
        SceneManager.LoadScene(0);
    }
}
