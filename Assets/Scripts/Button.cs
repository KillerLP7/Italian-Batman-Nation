using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    private int currentSceneIndex;

    private void Update()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void Back()
    {
        if (!(currentSceneIndex == 1 || currentSceneIndex == 2))
        {
            Options.OnExit();
        }
        SceneManager.LoadScene(4);
    }

    public void MainMenu()
    {
        if (!(currentSceneIndex == 1 || currentSceneIndex == 2))
        {
            Options.OnExit();
        }
        SceneManager.LoadScene(0);
    }
}
