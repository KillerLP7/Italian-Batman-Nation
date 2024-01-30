using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipScene : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene(2);
    }
}
