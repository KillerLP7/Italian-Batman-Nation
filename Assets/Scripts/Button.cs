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
        print("I am a button");

        print(currentSceneIndex);
        if (currentSceneIndex == 1)
        {
            SceneManager.LoadScene(4);
        }

        if (currentSceneIndex == 2)
        {
            GameManager.Instance.ResetEndlessWave();
            SceneManager.LoadScene(2);
        }
        
        if (!(currentSceneIndex == 1 || currentSceneIndex == 2))
        {
            print("I just got here");
            Options.OnExit();
        }
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
